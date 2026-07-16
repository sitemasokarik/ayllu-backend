namespace DcodePe.Catering.Application.ConsultaDocumento
{
    public class ConsultaDocumentoResult
    {
        public bool EsValido { get; set; }
        public string NumeroDocumento { get; set; } = string.Empty;
        public string? NombreORazonSocial { get; set; }
        public string? Direccion { get; set; }
        public string? Estado { get; set; }
        public string? Condicion { get; set; }
        public string? TipoDocumento { get; set; }
        public string? Mensaje { get; set; }
        public bool ConsultaApi { get; set; }
        public string? Fuente { get; set; }
        public string? ClienteTelefono { get; set; }
    }
}
