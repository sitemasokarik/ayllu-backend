namespace DcodePe.Catering.Application.DataBase.Cliente.Commands.Update
{
    public class UpdateClienteModel
    {
        public int ClienteID { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string TelefonoSecundario { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public string TipoCliente { get; set; }
        public string Observaciones { get; set; }
        public bool EsVIP { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
