using DcodePe.Catering.Domain.Entities.Base;

namespace DcodePe.Catering.Domain.Entities.Facturacion
{
    public class ComprobanteDetalleEntity : BaseEntity
    {
        public int ComprobanteDetalleID { get; set; }
        public int ComprobanteID { get; set; }
        public int Item { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string IdTipoIgv { get; set; } = "10";
        public string TipoIgv { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = "ZZ";
        public decimal Valor { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Importe { get; set; }

        public virtual ComprobanteElectronicoEntity Comprobante { get; set; } = null!;
    }
}
