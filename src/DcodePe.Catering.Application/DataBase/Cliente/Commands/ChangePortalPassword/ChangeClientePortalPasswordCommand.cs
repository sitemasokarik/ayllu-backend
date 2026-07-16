using DcodePe.Catering.Application.Security;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Cliente.Commands.ChangePortalPassword
{
    public class ChangeClientePortalPasswordCommand(
        IDataBaseService databaseService,
        IPasswordHashService passwordHashService) : IChangeClientePortalPasswordCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;
        private readonly IPasswordHashService _passwordHashService = passwordHashService;

        public async Task<bool> Execute(int clienteId, ChangeClientePortalPasswordModel model)
        {
            if (model.PasswordNueva.Length < 6)
                throw new InvalidOperationException("La nueva contraseña debe tener al menos 6 caracteres.");

            var cliente = await _databaseService.Cliente
                .FirstOrDefaultAsync(c => c.ClienteID == clienteId && c.Estado == true && c.EsPortalActivo);

            if (cliente == null || string.IsNullOrWhiteSpace(cliente.PasswordHash))
                throw new InvalidOperationException("Cliente no encontrado.");

            if (!_passwordHashService.VerifyPassword(model.PasswordActual, cliente.PasswordHash))
                throw new InvalidOperationException("La contraseña actual no es correcta.");

            cliente.PasswordHash = _passwordHashService.HashPassword(model.PasswordNueva);
            cliente.FechaModificacion = DateTime.Now;
            await _databaseService.SaveAsync();
            return true;
        }
    }
}
