using Microsoft.EntityFrameworkCore;
using DcodePe.Catering.Application.Security;

namespace DcodePe.Catering.Application.DataBase.Usuario.Commands.UpdatePassword
{
    public class UpdateUsuarioPasswordCommand : IUpdateUsuarioPasswordCommand
    {
        private readonly IDataBaseService _dataBaseService;
        private readonly IPasswordHashService _passwordHashService;

        public UpdateUsuarioPasswordCommand(
            IDataBaseService dataBaseService,
            IPasswordHashService passwordHashService)
        {
            _dataBaseService = dataBaseService;
            _passwordHashService = passwordHashService;
        }

        public async Task<bool> Execute(UpdateUsuarioPasswordModel model)
        {
            // Buscar el usuario
            var usuario = await _dataBaseService.Usuario
                .FirstOrDefaultAsync(u => u.UsuarioID == model.UsuarioID && u.Estado == true);

            if (usuario == null)
                return false;
            //Se comenta la verificacion de la contraseña actual para permitir el cambio sin ella
            // Verificar contraseña actual con BCrypt
            //bool isCurrentPasswordValid = _passwordHashService.VerifyPassword(
            //    model.CurrentPassword,
            //    usuario.Password);

            //if (!isCurrentPasswordValid)
            //    return false;

            // Hash de la nueva contraseña
            usuario.Password = _passwordHashService.HashPassword(model.NewPassword);
            usuario.FechaModificacion = DateTime.Now;
            usuario.UsuarioModificacion = usuario.UserName ?? "SYSTEM";

            await _dataBaseService.SaveAsync();
            return true;
        }
    }
}
