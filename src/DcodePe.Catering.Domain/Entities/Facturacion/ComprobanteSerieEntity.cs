namespace DcodePe.Catering.Domain.Entities.Facturacion
{
    public class ComprobanteSerieEntity
    {
        public int ComprobanteSerieID { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Serie { get; set; } = string.Empty;
        public int UltimoCorrelativo { get; set; }
    }
}
