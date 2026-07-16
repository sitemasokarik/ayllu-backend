using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.PagoVoucher.Queries.GetPendientes
{
    public class PagoVoucherPendienteModel
    {
        public int PagoVoucherID { get; set; }
        public int CotizacionID { get; set; }
        public int ClienteID { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public string ClienteEmail { get; set; } = string.Empty;
        public string LocalNombre { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string ArchivoUrl { get; set; } = string.Empty;
        public string NombreArchivo { get; set; } = string.Empty;
        public string? ObservacionCliente { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaTentativa { get; set; }
        public DateTime? FechaTentativaOpcional { get; set; }
        public DateTime? FechaReservadaElegida { get; set; }
    }

    public interface IGetPagoVoucherPendientesQuery
    {
        Task<List<PagoVoucherPendienteModel>> Execute();
        Task<int> CountPendientesAsync();
    }

    public class GetPagoVoucherPendientesQuery(IDataBaseService databaseService) : IGetPagoVoucherPendientesQuery
    {
        private readonly IDataBaseService _databaseService = databaseService;

        public async Task<List<PagoVoucherPendienteModel>> Execute()
        {
            return await _databaseService.PagoVoucher
                .AsNoTracking()
                .Where(v => v.Estado == true && v.EstadoPago == "Pendiente")
                .Include(v => v.Cliente)
                .Include(v => v.Cotizacion)
                    .ThenInclude(c => c.Local)
                .OrderByDescending(v => v.FechaCreacion)
                .Select(v => new PagoVoucherPendienteModel
                {
                    PagoVoucherID = v.PagoVoucherID,
                    CotizacionID = v.CotizacionID,
                    ClienteID = v.ClienteID,
                    ClienteNombre = v.Cliente.NombreCompleto,
                    ClienteEmail = v.Cliente.Email,
                    LocalNombre = v.Cotizacion.Local.Nombre,
                    Monto = v.Monto,
                    ArchivoUrl = v.ArchivoUrl,
                    NombreArchivo = v.NombreArchivo,
                    ObservacionCliente = v.ObservacionCliente,
                    FechaCreacion = v.FechaCreacion,
                    FechaTentativa = v.Cotizacion.FechaTentativa,
                    FechaTentativaOpcional = v.Cotizacion.FechaTentativaOpcional,
                    FechaReservadaElegida = v.FechaReservadaElegida
                })
                .ToListAsync();
        }

        public Task<int> CountPendientesAsync()
        {
            return _databaseService.PagoVoucher
                .CountAsync(v => v.Estado == true && v.EstadoPago == "Pendiente");
        }
    }
}
