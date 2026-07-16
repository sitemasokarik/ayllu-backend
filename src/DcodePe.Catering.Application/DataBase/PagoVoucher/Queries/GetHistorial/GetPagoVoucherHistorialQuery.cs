using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.PagoVoucher.Queries.GetHistorial
{
    public class PagoVoucherHistorialModel
    {
        public int PagoVoucherID { get; set; }
        public int CotizacionID { get; set; }
        public int ClienteID { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public string ClienteEmail { get; set; } = string.Empty;
        public string LocalNombre { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string EstadoPago { get; set; } = string.Empty;
        public string? ObservacionAdmin { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaTentativa { get; set; }
        public DateTime? FechaTentativaOpcional { get; set; }
        public DateTime? FechaReservadaElegida { get; set; }
    }

    public interface IGetPagoVoucherHistorialQuery
    {
        Task<List<PagoVoucherHistorialModel>> Execute(string? estadoPago = null);
    }

    public class GetPagoVoucherHistorialQuery(IDataBaseService databaseService) : IGetPagoVoucherHistorialQuery
    {
        private readonly IDataBaseService _databaseService = databaseService;

        public async Task<List<PagoVoucherHistorialModel>> Execute(string? estadoPago = null)
        {
            var query = _databaseService.PagoVoucher
                .AsNoTracking()
                .Where(v => v.Estado == true);

            if (!string.IsNullOrWhiteSpace(estadoPago))
                query = query.Where(v => v.EstadoPago == estadoPago);

            return await query
                .Include(v => v.Cliente)
                .Include(v => v.Cotizacion)
                    .ThenInclude(c => c.Local)
                .OrderByDescending(v => v.FechaCreacion)
                .Select(v => new PagoVoucherHistorialModel
                {
                    PagoVoucherID = v.PagoVoucherID,
                    CotizacionID = v.CotizacionID,
                    ClienteID = v.ClienteID,
                    ClienteNombre = v.Cliente.NombreCompleto,
                    ClienteEmail = v.Cliente.Email,
                    LocalNombre = v.Cotizacion.Local.Nombre,
                    Monto = v.Monto,
                    EstadoPago = v.EstadoPago,
                    ObservacionAdmin = v.ObservacionAdmin,
                    FechaCreacion = v.FechaCreacion,
                    FechaModificacion = v.FechaModificacion,
                    FechaTentativa = v.Cotizacion.FechaTentativa,
                    FechaTentativaOpcional = v.Cotizacion.FechaTentativaOpcional,
                    FechaReservadaElegida = v.FechaReservadaElegida
                })
                .ToListAsync();
        }
    }
}
