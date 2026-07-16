using DcodePe.Catering.Application.Database;
using DcodePe.Catering.Application.DataBase.Cotizacion.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace DcodePe.Catering.Application.DataBase.MercadoPago.Commands.ProcessWebhook
{
    public class MercadoPagoWebhookModel
    {
        public string? Topic { get; set; }
        public string? ResourceId { get; set; }
        public string? XRequestId { get; set; }
        public string? XSignature { get; set; }
    }

    public interface IProcessMercadoPagoWebhookCommand
    {
        Task<bool> Execute(MercadoPagoWebhookModel model);
    }

    public class ProcessMercadoPagoWebhookCommand(
        IDataBaseService databaseService,
        IConfiguration configuration,
        ILogger<ProcessMercadoPagoWebhookCommand> logger) : IProcessMercadoPagoWebhookCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<ProcessMercadoPagoWebhookCommand> _logger = logger;

        public async Task<bool> Execute(MercadoPagoWebhookModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ResourceId))
                return false;

            if (!ValidateSignature(model))
            {
                _logger.LogWarning("Webhook MP rechazado: firma inválida.");
                return false;
            }

            var accessToken = _configuration["MercadoPago:AccessToken"];
            if (string.IsNullOrWhiteSpace(accessToken))
                return false;

            using var http = new HttpClient();
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var paymentResponse = await http.GetAsync($"https://api.mercadopago.com/v1/payments/{model.ResourceId}");
            if (!paymentResponse.IsSuccessStatusCode)
                return false;

            var paymentJson = await paymentResponse.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(paymentJson);
            var root = doc.RootElement;

            var paymentId = root.GetProperty("id").GetRawText().Trim('"');
            var status = root.GetProperty("status").GetString() ?? string.Empty;
            var statusDetail = root.TryGetProperty("status_detail", out var sd) ? sd.GetString() : null;
            var externalRef = root.TryGetProperty("external_reference", out var er) ? er.GetString() : null;
            var transactionAmount = root.TryGetProperty("transaction_amount", out var ta) ? ta.GetDecimal() : 0m;

            if (string.IsNullOrWhiteSpace(externalRef) || !externalRef.StartsWith("COT-", StringComparison.OrdinalIgnoreCase))
                return false;

            var parts = externalRef.Split('-');
            if (parts.Length < 2 || !int.TryParse(parts[1], out var cotizacionId))
                return false;

            var existing = await _databaseService.PagoMercadoPago
                .FirstOrDefaultAsync(p => p.MpPaymentId == paymentId && p.Estado == true);

            if (existing != null)
                return true;

            var empresa = await _databaseService.Empresa
                .Where(e => e.Estado == true)
                .OrderBy(e => e.EmpresaID)
                .FirstOrDefaultAsync();
            var montoEsperado = empresa?.MontoAdelantoReserva > 0 ? empresa!.MontoAdelantoReserva!.Value : 1000m;

            if (Math.Abs(transactionAmount - montoEsperado) > 0.01m)
            {
                _logger.LogWarning("Webhook MP: monto {Monto} distinto al adelanto {Esperado}", transactionAmount, montoEsperado);
                return false;
            }

            var cotizacion = await _databaseService.Cotizacion
                .FirstOrDefaultAsync(c => c.CotizacionID == cotizacionId && c.Estado == true);

            if (cotizacion == null)
                return false;

            var pending = await _databaseService.PagoMercadoPago
                .Where(p => p.CotizacionID == cotizacionId && p.Estado == true && p.MpPaymentId.StartsWith("PREF-"))
                .OrderByDescending(p => p.FechaCreacion)
                .FirstOrDefaultAsync();

            if (pending != null)
            {
                pending.MpPaymentId = paymentId;
                pending.EstadoPago = MapStatus(status);
                pending.MpStatusDetail = statusDetail;
                pending.FechaPago = DateTime.UtcNow;
                pending.FechaModificacion = DateTime.UtcNow;
                pending.UsuarioModificacion = "webhook-mp";
            }
            else
            {
                var preferenceId = root.TryGetProperty("order", out var order)
                    && order.TryGetProperty("id", out var orderId)
                    ? orderId.GetString() ?? string.Empty
                    : string.Empty;

                await _databaseService.PagoMercadoPago.AddAsync(new Domain.Entities.Pagos.PagoMercadoPagoEntity
                {
                    CotizacionID = cotizacionId,
                    ClienteID = cotizacion.ClienteID,
                    Monto = transactionAmount,
                    MpPaymentId = paymentId,
                    MpPreferenceId = preferenceId,
                    EstadoPago = MapStatus(status),
                    MpStatusDetail = statusDetail,
                    FechaPago = DateTime.UtcNow,
                    UsuarioCreacion = "webhook-mp",
                    FechaCreacion = DateTime.UtcNow,
                    Estado = true
                });
            }

            if (string.Equals(status, "approved", StringComparison.OrdinalIgnoreCase))
            {
                cotizacion.EstadoCotizacion = "Evento";
                var fechaElegida = pending?.FechaReservadaElegida ?? cotizacion.FechaReservada;
                if (fechaElegida.HasValue)
                {
                    cotizacion.FechaReservada = fechaElegida.Value.Date;
                }
                else
                {
                    var disponibles = FechaReservaHelper.ObtenerFechasDisponibles(cotizacion);
                    if (disponibles.Count == 1)
                        cotizacion.FechaReservada = disponibles[0];
                }
                cotizacion.FechaModificacion = DateTime.Now;
                cotizacion.UsuarioModificacion = "mercadopago";
            }

            await _databaseService.SaveAsync();
            return true;
        }

        private bool ValidateSignature(MercadoPagoWebhookModel model)
        {
            var secret = _configuration["MercadoPago:WebhookSecret"];
            if (string.IsNullOrWhiteSpace(secret))
                return true;

            if (string.IsNullOrWhiteSpace(model.XSignature))
                return false;

            var parts = model.XSignature.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var ts = parts.FirstOrDefault(p => p.StartsWith("ts=", StringComparison.OrdinalIgnoreCase))?.Split('=')[1];
            var v1 = parts.FirstOrDefault(p => p.StartsWith("v1=", StringComparison.OrdinalIgnoreCase))?.Split('=')[1];

            if (string.IsNullOrWhiteSpace(ts) || string.IsNullOrWhiteSpace(v1))
                return false;

            var manifest = $"id:{model.ResourceId};request-id:{model.XRequestId};ts:{ts};";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(manifest))).ToLowerInvariant();
            return hash.Equals(v1, StringComparison.OrdinalIgnoreCase);
        }

        private static string MapStatus(string status) => status.ToLowerInvariant() switch
        {
            "approved" => "Aprobado",
            "rejected" => "Rechazado",
            "cancelled" => "Cancelado",
            "in_process" or "pending" => "Pendiente",
            _ => status
        };
    }
}
