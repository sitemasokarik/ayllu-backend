namespace DcodePe.Catering.Application.DataBase.Cliente.Queries.LoginPortal
{
    public class LoginClientePortalModel
    {
        public string Token { get; set; } = string.Empty;
        public int ClienteID { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public string? Telefono { get; set; }
    }
}
