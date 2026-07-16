namespace DcodePe.Catering.Application.DataBase.Comprobante.Commands.Create
{
    public class CreateComprobanteItemModel
    {
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
    }

    public class CreateComprobanteModel
    {
        public int ComprobanteID { get; set; }
        public string Tipo { get; set; } = "boleta";
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
        public string FormaPago { get; set; } = "Contado";
        public string MedioPago { get; set; } = "EFECTIVO";
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
        public string UsuarioCreacion { get; set; } = "SYSTEM";
        public List<CreateComprobanteItemModel> Items { get; set; } = new();
    }
}
