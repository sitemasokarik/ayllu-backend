namespace DcodePe.Catering.Application.DataBase.PagoVoucher.Commands.UploadPortal
{
    public class UploadPagoVoucherPortalModel
    {
        public int CotizacionID { get; set; }
        public int ClienteID { get; set; }
        public string ArchivoUrl { get; set; } = string.Empty;
        public string NombreArchivo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime? FechaReservadaElegida { get; set; }
        public string? ObservacionCliente { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
    }

    public interface IUploadPagoVoucherPortalCommand
    {
        Task<UploadPagoVoucherPortalModel> Execute(UploadPagoVoucherPortalModel model);
    }
}
