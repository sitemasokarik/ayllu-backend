namespace DcodePe.Catering.Application.DataBase.Comprobante.Queries.GetAllComprobante
{
    public class GetAllComprobanteItemModel
    {
        public int ComprobanteDetalleID { get; set; }
        public int Item { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string IdTipoIgv { get; set; } = string.Empty;
        public string TipoIgv { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Importe { get; set; }
    }

    public class GetAllComprobanteModel
    {
        public int ComprobanteID { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Serie { get; set; } = string.Empty;
        public string Correlativo { get; set; } = string.Empty;
        public string NumeroCompleto { get; set; } = string.Empty;
        public int? CotizacionID { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public string ClienteDocumento { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string? ClienteDireccion { get; set; }
        public string? ClienteTelefono { get; set; }
        public DateTime FechaEmision { get; set; }
        public string FormaPago { get; set; } = string.Empty;
        public string MedioPago { get; set; } = string.Empty;
        public string Moneda { get; set; } = string.Empty;
        public decimal OpGravadas { get; set; }
        public decimal OpInafectas { get; set; }
        public decimal OpExoneradas { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }
        public decimal Recibido { get; set; }
        public decimal Vuelto { get; set; }
        public string ModoEmision { get; set; } = string.Empty;
        public string EstadoComprobante { get; set; } = string.Empty;
        public string? SunatTicket { get; set; }
        public string? SunatCdr { get; set; }
        public string? SunatRespuesta { get; set; }
        public string? SunatCodigoError { get; set; }
        public string? RutaXml { get; set; }
        public string? RutaCdr { get; set; }
        public decimal MontoAdelantoFacturado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public List<GetAllComprobanteItemModel> Items { get; set; } = new();
    }
}
