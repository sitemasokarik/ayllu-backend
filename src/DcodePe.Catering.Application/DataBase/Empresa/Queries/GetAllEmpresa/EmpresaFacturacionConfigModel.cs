namespace DcodePe.Catering.Application.DataBase.Empresa.Queries.GetAllEmpresa
{
    public class EmpresaFacturacionConfigModel
    {
        public int EmpresaID { get; set; }
        public string RazonSocial { get; set; } = string.Empty;
        public string NombreComercial { get; set; } = string.Empty;
        public string RUC { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Ubigeo { get; set; }
        public bool GeneraFactElect { get; set; }
        public bool SunatConfigurado { get; set; }
        public string SunatModo { get; set; } = "DESARROLLO";
        public bool SunatIntegrado { get; set; }
        public string SunatWsUrlActivo { get; set; } = string.Empty;
        public string RutaCertificadoServidor { get; set; } = "fe/certificado";
        public string? CertificadoFileName { get; set; }
        public string? UsuarioSol { get; set; }
    }
}
