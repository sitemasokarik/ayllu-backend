namespace DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Claim
{
    public interface IClaimCotizacionCommand
    {
        Task<ClaimCotizacionResult> Execute(int cotizacionId, int usuarioId, string usuarioNombre);
    }

    public class ClaimCotizacionResult
    {
        public bool Success { get; set; }
        public bool AlreadyAssigned { get; set; }
        public string? ResponsableNombre { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public string? Message { get; set; }
    }
}
