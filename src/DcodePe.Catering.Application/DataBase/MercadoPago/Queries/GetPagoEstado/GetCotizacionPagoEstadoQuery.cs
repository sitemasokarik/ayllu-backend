using DcodePe.Catering.Application.Database;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.MercadoPago.Queries.GetPagoEstado
{
    public class CotizacionPagoEstadoModel
    {
        public int CotizacionID { get; set; }
        public string EstadoCotizacion { get; set; } = string.Empty;
        public decimal MontoAdelantoEsperado { get; set; }
        public PagoVoucherEstadoModel? Voucher { get; set; }
        public PagoMercadoPagoEstadoModel? MercadoPago { get; set; }
    }

    public class PagoVoucherEstadoModel
    {
        public int PagoVoucherID { get; set; }
        public decimal Monto { get; set; }
        public string EstadoPago { get; set; } = string.Empty;
        public DateTime? FechaCreacion { get; set; }
    }

    public class PagoMercadoPagoEstadoModel
    {
        public int PagoMercadoPagoID { get; set; }
        public decimal Monto { get; set; }
        public string EstadoPago { get; set; } = string.Empty;
        public string MpPaymentId { get; set; } = string.Empty;
        public string MpPreferenceId { get; set; } = string.Empty;
        public DateTime? FechaPago { get; set; }
    }

    public interface IGetCotizacionPagoEstadoQuery
    {
        Task<CotizacionPagoEstadoModel?> Execute(int cotizacionId, int clienteId);
    }

    public class GetCotizacionPagoEstadoQuery(IDataBaseService databaseService) : IGetCotizacionPagoEstadoQuery
    {
        private readonly IDataBaseService _databaseService = databaseService;

        public async Task<CotizacionPagoEstadoModel?> Execute(int cotizacionId, int clienteId)
        {
            var cotizacion = await _databaseService.Cotizacion
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CotizacionID == cotizacionId && c.ClienteID == clienteId && c.Estado == true);

            if (cotizacion == null)
                return null;

            var empresa = await _databaseService.Empresa
                .AsNoTracking()
                .Where(e => e.Estado == true)
                .OrderBy(e => e.EmpresaID)
                .FirstOrDefaultAsync();

            var voucher = await _databaseService.PagoVoucher
                .AsNoTracking()
                .Where(v => v.CotizacionID == cotizacionId && v.Estado == true)
                .OrderByDescending(v => v.FechaCreacion)
                .FirstOrDefaultAsync();

            var mp = await _databaseService.PagoMercadoPago
                .AsNoTracking()
                .Where(p => p.CotizacionID == cotizacionId && p.Estado == true && !p.MpPaymentId.StartsWith("PREF-"))
                .OrderByDescending(p => p.FechaPago ?? p.FechaCreacion)
                .FirstOrDefaultAsync();

            if (mp == null)
            {
                mp = await _databaseService.PagoMercadoPago
                    .AsNoTracking()
                    .Where(p => p.CotizacionID == cotizacionId && p.Estado == true)
                    .OrderByDescending(p => p.FechaCreacion)
                    .FirstOrDefaultAsync();
            }

            return new CotizacionPagoEstadoModel
            {
                CotizacionID = cotizacionId,
                EstadoCotizacion = cotizacion.EstadoCotizacion ?? string.Empty,
                MontoAdelantoEsperado = empresa?.MontoAdelantoReserva > 0 ? empresa!.MontoAdelantoReserva!.Value : 1000m,
                Voucher = voucher == null ? null : new PagoVoucherEstadoModel
                {
                    PagoVoucherID = voucher.PagoVoucherID,
                    Monto = voucher.Monto,
                    EstadoPago = voucher.EstadoPago,
                    FechaCreacion = voucher.FechaCreacion
                },
                MercadoPago = mp == null ? null : new PagoMercadoPagoEstadoModel
                {
                    PagoMercadoPagoID = mp.PagoMercadoPagoID,
                    Monto = mp.Monto,
                    EstadoPago = mp.EstadoPago,
                    MpPaymentId = mp.MpPaymentId,
                    MpPreferenceId = mp.MpPreferenceId,
                    FechaPago = mp.FechaPago ?? mp.FechaCreacion
                }
            };
        }
    }
}
