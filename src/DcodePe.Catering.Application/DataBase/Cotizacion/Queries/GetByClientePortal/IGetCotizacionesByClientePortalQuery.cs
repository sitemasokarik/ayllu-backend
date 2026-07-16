namespace DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetByClientePortal
{
    public class GetCotizacionPortalModel
    {
        public int CotizacionID { get; set; }
        public int LocalID { get; set; }
        public DateTime? FechaTentativa { get; set; }
        public DateTime? FechaTentativaOpcional { get; set; }
        public DateTime? FechaReservada { get; set; }
        public int NumeroInvitados { get; set; }
        public decimal TotalCotizacion { get; set; }
        public decimal TotalEvento { get; set; }
        public string EstadoCotizacion { get; set; } = string.Empty;
        public string LocalNombre { get; set; } = string.Empty;
        public string? EventoNombre { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool PuedePagar { get; set; }
        public bool VoucherPendiente { get; set; }
        public string? UltimoVoucherEstado { get; set; }
        public string? UltimoVoucherUrl { get; set; }
        public string? UltimoVoucherObservacionAdmin { get; set; }
    }

    public interface IGetCotizacionesByClientePortalQuery
    {
        Task<List<GetCotizacionPortalModel>> Execute(int clienteId);
    }
}
