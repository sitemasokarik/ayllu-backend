namespace DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetCotizacionFacturacion
{
    public class CotizacionFacturacionModel
    {
        public int CotizacionID { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public string ClienteDocumento { get; set; } = string.Empty;
        public string? ClienteDireccion { get; set; }
        public string? ClienteTelefono { get; set; }
        public decimal MontoAdelanto { get; set; }
        public int? PagoVoucherID { get; set; }
        public int? PagoMercadoPagoID { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public bool YaFacturado { get; set; }
    }

    public interface IGetCotizacionFacturacionQuery
    {
        Task<CotizacionFacturacionModel> Execute(int cotizacionId);
    }
}
