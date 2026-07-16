namespace DcodePe.Catering.Application.DataBase.PagoVoucher.Commands.Review
{
    public class ReviewPagoVoucherModel
    {
        public int PagoVoucherID { get; set; }
        public bool Aprobado { get; set; }
        public string? ObservacionAdmin { get; set; }
        public DateTime? FechaReservadaElegida { get; set; }
        public string UsuarioModificacion { get; set; } = "admin";
    }

    public interface IReviewPagoVoucherCommand
    {
        Task<bool> Execute(ReviewPagoVoucherModel model);
    }
}
