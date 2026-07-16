using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetCotizacionFacturacion
{
    public class GetCotizacionFacturacionQuery(IDataBaseService databaseService) : IGetCotizacionFacturacionQuery
    {
        private readonly IDataBaseService _databaseService = databaseService;

        public async Task<CotizacionFacturacionModel> Execute(int cotizacionId)
        {
            var cotizacion = await _databaseService.Cotizacion
                .AsNoTracking()
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(c => c.CotizacionID == cotizacionId && c.Estado == true);

            if (cotizacion == null)
                throw new InvalidOperationException("Cotización no encontrada.");

            if (!string.Equals(cotizacion.EstadoCotizacion, "Evento", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Solo se puede facturar una cotización en estado Evento.");

            var comprobanteExistente = await _databaseService.ComprobanteElectronico
                .AsNoTracking()
                .AnyAsync(c => c.CotizacionID == cotizacionId && c.Estado == true);

            var voucher = await _databaseService.PagoVoucher
                .AsNoTracking()
                .Where(v =>
                    v.CotizacionID == cotizacionId &&
                    (v.Estado == true || v.Estado == null) &&
                    v.EstadoPago == "Aprobado")
                .OrderByDescending(v => v.FechaModificacion ?? v.FechaCreacion)
                .FirstOrDefaultAsync();

            Domain.Entities.Pagos.PagoMercadoPagoEntity? mp = null;
            try
            {
                mp = await _databaseService.PagoMercadoPago
                    .AsNoTracking()
                    .Where(p =>
                        p.CotizacionID == cotizacionId &&
                        (p.Estado == true || p.Estado == null) &&
                        p.EstadoPago == "Aprobado")
                    .OrderByDescending(p => p.FechaPago ?? p.FechaCreacion)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                // Bases SQLite antiguas pueden no tener aún la tabla PagoMercadoPago.
            }

            Domain.Entities.Pagos.PagoVoucherEntity? pagoVoucher = null;
            Domain.Entities.Pagos.PagoMercadoPagoEntity? pagoMp = null;
            string metodo = string.Empty;

            if (voucher != null && mp != null)
            {
                var fechaVoucher = voucher.FechaModificacion ?? voucher.FechaCreacion ?? DateTime.MinValue;
                var fechaMp = mp.FechaPago ?? mp.FechaCreacion ?? DateTime.MinValue;
                if (fechaMp >= fechaVoucher)
                {
                    pagoMp = mp;
                    metodo = "MercadoPago";
                }
                else
                {
                    pagoVoucher = voucher;
                    metodo = "Voucher";
                }
            }
            else if (mp != null)
            {
                pagoMp = mp;
                metodo = "MercadoPago";
            }
            else if (voucher != null)
            {
                pagoVoucher = voucher;
                metodo = "Voucher";
            }

            if (pagoVoucher == null && pagoMp == null)
            {
                var ultimoVoucher = await _databaseService.PagoVoucher
                    .AsNoTracking()
                    .Where(v => v.CotizacionID == cotizacionId && (v.Estado == true || v.Estado == null))
                    .OrderByDescending(v => v.FechaModificacion ?? v.FechaCreacion)
                    .FirstOrDefaultAsync();

                if (ultimoVoucher != null &&
                    string.Equals(ultimoVoucher.EstadoPago, "Pendiente", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        "El adelanto con voucher aún está pendiente de aprobación. Revise Facturación → Pagos vouchers.");
                }

                throw new InvalidOperationException("No hay adelanto aprobado para esta cotización.");
            }

            if (comprobanteExistente)
            {
                var comprobante = await _databaseService.ComprobanteElectronico
                    .AsNoTracking()
                    .Where(c => c.CotizacionID == cotizacionId && c.Estado == true)
                    .OrderByDescending(c => c.FechaCreacion)
                    .Select(c => c.NumeroCompleto)
                    .FirstOrDefaultAsync();

                var detalle = string.IsNullOrWhiteSpace(comprobante)
                    ? string.Empty
                    : $" ({comprobante})";

                throw new InvalidOperationException(
                    $"Ya existe un comprobante activo para esta cotización{detalle}.");
            }

            var monto = pagoMp?.Monto ?? pagoVoucher!.Monto;

            return new CotizacionFacturacionModel
            {
                CotizacionID = cotizacionId,
                ClienteNombre = cotizacion.Cliente?.NombreCompleto ?? string.Empty,
                ClienteDocumento = cotizacion.Cliente?.NumeroDocumento ?? string.Empty,
                ClienteDireccion = cotizacion.Cliente?.Direccion,
                ClienteTelefono = cotizacion.Cliente?.Telefono,
                MontoAdelanto = monto,
                PagoVoucherID = pagoVoucher?.PagoVoucherID,
                PagoMercadoPagoID = pagoMp?.PagoMercadoPagoID,
                MetodoPago = metodo,
                YaFacturado = false
            };
        }
    }
}
