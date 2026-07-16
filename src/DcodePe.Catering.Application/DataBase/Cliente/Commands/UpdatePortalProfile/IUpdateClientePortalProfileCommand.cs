namespace DcodePe.Catering.Application.DataBase.Cliente.Commands.UpdatePortalProfile
{
    public class UpdateClientePortalProfileModel
    {
        public int ClienteID { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
    }

    public interface IUpdateClientePortalProfileCommand
    {
        Task<UpdateClientePortalProfileModel> Execute(int clienteId, UpdateClientePortalProfileModel model);
    }
}
