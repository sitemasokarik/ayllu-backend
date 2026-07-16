namespace DcodePe.Catering.Application.DataBase.Cliente.Commands.ChangePortalPassword
{
    public class ChangeClientePortalPasswordModel
    {
        public string PasswordActual { get; set; } = string.Empty;
        public string PasswordNueva { get; set; } = string.Empty;
    }

    public interface IChangeClientePortalPasswordCommand
    {
        Task<bool> Execute(int clienteId, ChangeClientePortalPasswordModel model);
    }
}
