using DcodePe.Catering.Application.DataBase.Cotizacion.Helpers;
using DcodePe.Catering.Application.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace DcodePe.Catering.Application.DataBase.MercadoPago.Commands.CreatePreference
{
    public class CreateMercadoPagoPreferenceModel
    {
        public int CotizacionID { get; set; }
        public int ClienteID { get; set; }
        public decimal Monto { get; set; }
        public string PreferenceId { get; set; } = string.Empty;
        public string InitPoint { get; set; } = string.Empty;
    }

    public interface ICreateMercadoPagoPreferenceCommand
    {
        Task<CreateMercadoPagoPreferenceModel> Execute(int cotizacionId, int clienteId, DateTime? fechaReservadaElegida);
    }

    public class CreateMercadoPagoPreferenceCommand(
        IDataBaseService databaseService,
        IConfiguration configuration,
        ILogger<CreateMercadoPagoPreferenceCommand> logger) : ICreateMercadoPagoPreferenceCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<CreateMercadoPagoPreferenceCommand> _logger = logger;

        public async Task<CreateMercadoPagoPreferenceModel> Execute(int cotizacionId, int clienteId, DateTime? fechaReservadaElegida)
        {
            var accessToken = _configuration["MercadoPago:AccessToken"];
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new InvalidOperationException("Mercado Pago no está configurado.");

            var cotizacion = await _databaseService.Cotizacion
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(c => c.CotizacionID == cotizacionId && c.Estado == true);

            if (cotizacion == null || cotizacion.ClienteID != clienteId)
                throw new InvalidOperationException("Cotización no encontrada.");

            if (!string.Equals(cotizacion.EstadoCotizacion, "Activo", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Solo puedes pagar cotizaciones en estado Activo.");

            var empresa = await _databaseService.Empresa
                .Where(e => e.Estado == true)
                .OrderBy(e => e.EmpresaID)
                .FirstOrDefaultAsync();

            var montoAdelanto = empresa?.MontoAdelantoReserva > 0 ? empresa!.MontoAdelantoReserva!.Value : 1000m;

            var pendingMp = await _databaseService.PagoMercadoPago
                .AnyAsync(p => p.CotizacionID == cotizacionId && p.Estado == true && p.EstadoPago == "Pendiente");

            if (pendingMp)
                throw new InvalidOperationException("Ya existe un pago con Mercado Pago pendiente para esta cotización.");

            var fechaReservada = FechaReservaHelper.ResolverFechaReservada(cotizacion, fechaReservadaElegida);

            var successUrl = _configuration["MercadoPago:SuccessUrl"] ?? "http://localhost:4200/portal/cotizaciones?mp=ok";
            var failureUrl = _configuration["MercadoPago:FailureUrl"] ?? "http://localhost:4200/portal/cotizaciones?mp=fail";
            var pendingUrl = _configuration["MercadoPago:PendingUrl"] ?? "http://localhost:4200/portal/cotizaciones?mp=pending";
            var notificationUrl = _configuration["MercadoPago:NotificationUrl"];

            var preferenceRequest = new
            {
                items = new[]
                {
                    new
                    {
                        title = $"Adelanto reserva - Cot. #{cotizacionId}",
                        quantity = 1,
                        currency_id = "PEN",
                        unit_price = montoAdelanto
                    }
                },
                payer = new
                {
                    email = cotizacion.Cliente?.Email ?? "cliente@ayllu.pe"
                },
                external_reference = $"COT-{cotizacionId}-CLI-{clienteId}",
                back_urls = new
                {
                    success = successUrl,
                    failure = failureUrl,
                    pending = pendingUrl
                },
                auto_return = "approved",
                notification_url = notificationUrl,
                statement_descriptor = "AYLLU ADELANTO"
            };

            using var http = new HttpClient();
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var json = System.Text.Json.JsonSerializer.Serialize(preferenceRequest);
            using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await http.PostAsync("https://api.mercadopago.com/checkout/preferences", content);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Mercado Pago preference error: {Status} {Body}", response.StatusCode, body);
                throw new InvalidOperationException("No se pudo crear la preferencia de pago.");
            }

            using var doc = System.Text.Json.JsonDocument.Parse(body);
            var preferenceId = doc.RootElement.GetProperty("id").GetString() ?? string.Empty;
            var initPoint = doc.RootElement.TryGetProperty("init_point", out var ip)
                ? ip.GetString() ?? string.Empty
                : string.Empty;

            var entity = new Domain.Entities.Pagos.PagoMercadoPagoEntity
            {
                CotizacionID = cotizacionId,
                ClienteID = clienteId,
                Monto = montoAdelanto,
                MpPreferenceId = preferenceId,
                MpPaymentId = $"PREF-{preferenceId}",
                FechaReservadaElegida = fechaReservada,
                EstadoPago = "Pendiente",
                UsuarioCreacion = cotizacion.Cliente?.Email ?? "portal",
                FechaCreacion = DateTime.UtcNow,
                Estado = true
            };

            await _databaseService.PagoMercadoPago.AddAsync(entity);
            await _databaseService.SaveAsync();

            return new CreateMercadoPagoPreferenceModel
            {
                CotizacionID = cotizacionId,
                ClienteID = clienteId,
                Monto = montoAdelanto,
                PreferenceId = preferenceId,
                InitPoint = initPoint
            };
        }
    }
}
