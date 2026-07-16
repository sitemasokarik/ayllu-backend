namespace DcodePe.Catering.External.Sunat
{
    public class SunatEmpresaConfig
    {
        public string Ruc { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string Ubigeo { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string UsuarioSol { get; set; } = string.Empty;
        public string ClaveSol { get; set; } = string.Empty;
        public string CertificadoPath { get; set; } = string.Empty;
        public string ClaveCertificado { get; set; } = string.Empty;
        public string WsUrl { get; set; } = string.Empty;
        public string SunatModo { get; set; } = "DESARROLLO";
        public bool GeneraFactElect { get; set; }
    }
}
