using DcodePe.Catering.Domain.Entities.Base;

namespace DcodePe.Catering.Domain.Entities.Facturacion
{
    public class ComprobanteElectronicoEntity : BaseEntity
    {
        public int ComprobanteID { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Serie { get; set; } = string.Empty;
        public string Correlativo { get; set; } = string.Empty;
        public string NumeroCompleto { get; set; } = string.Empty;
        public int? CotizacionID { get; set; }
        public int? PagoVoucherID { get; set; }
        public int? PagoMercadoPagoID { get; set; }
        public decimal MontoAdelantoFacturado { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public string ClienteDocumento { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string? ClienteDireccion { get; set; }
        public string? ClienteTelefono { get; set; }
        public DateTime FechaEmision { get; set; }
        public string FormaPago { get; set; } = string.Empty;
        public string MedioPago { get; set; } = string.Empty;
        public string Moneda { get; set; } = "PEN";
        public decimal OpGravadas { get; set; }
        public decimal OpInafectas { get; set; }
        public decimal OpExoneradas { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }
        public decimal Recibido { get; set; }
        public decimal Vuelto { get; set; }
        public string ModoEmision { get; set; } = "solo_venta";
        public string EstadoComprobante { get; set; } = "Registrado";
        public string? SunatTicket { get; set; }
        public string? SunatCdr { get; set; }
        public string? SunatRespuesta { get; set; }
        public string? SunatHashCpe { get; set; }
        public string? RutaXml { get; set; }
        public string? RutaCdr { get; set; }
        public string? SunatCodigoError { get; set; }

        public virtual ICollection<ComprobanteDetalleEntity> Detalles { get; set; } = new List<ComprobanteDetalleEntity>();
    }
}
