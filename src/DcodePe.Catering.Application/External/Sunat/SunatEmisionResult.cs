namespace DcodePe.Catering.Application.External.Sunat
{
    public class SunatEmisionResult
    {
        public bool Integrado { get; set; }
        public string? Ticket { get; set; }
        public string EstadoComprobante { get; set; } = "Registrado";
        public string? Respuesta { get; set; }
        public string? SunatHashCpe { get; set; }
        public string? RutaXml { get; set; }
        public string? RutaCdr { get; set; }
        public string? SunatCodigoError { get; set; }
        public string? SunatCdr { get; set; }
    }
}
