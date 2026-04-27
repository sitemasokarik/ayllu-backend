using Microsoft.EntityFrameworkCore;
using DcodePe.Catering.Domain.Entities;

namespace DcodePe.Catering.Application.DataBase.Usuario.Commands.Update
{
    public class UpdateUsuarioCommand : IUpdateUsuarioCommand
    {
        private readonly IDataBaseService _databaseService;

        public UpdateUsuarioCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(UpdateUsuarioModel model)
        {
            var entity = await _databaseService.Usuario
                .FirstOrDefaultAsync(u => u.UsuarioID == model.UsuarioID);

            if (entity == null)
                return false;

            // Actualizar campos (NO actualiza password aquí, usa UpdatePassword)
            entity.Nombre = model.Nombre;
            entity.UserName = model.UserName;
            entity.Email = model.Email;
            entity.RolID = model.RolID;
            entity.UsuarioModificacion = model.UsuarioModificacion ?? "SYSTEM";
            entity.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
