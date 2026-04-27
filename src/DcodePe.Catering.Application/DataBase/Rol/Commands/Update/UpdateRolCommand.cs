using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Rol.Commands.Update
{
    public class UpdateRolCommand : IUpdateRolCommand
    {
        private readonly IDataBaseService _databaseService;

        public UpdateRolCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(UpdateRolModel model)
        {
            var entity = await _databaseService.Rol
                .FirstOrDefaultAsync(r => r.RolID == model.RolID && r.Estado == true);

            if (entity == null)
                return false;

            // Validar nombre único (excepto el actual)
            var existeRol = await _databaseService.Rol
                .AnyAsync(r => r.Nombre == model.Nombre && 
                              r.RolID != model.RolID && 
                              r.Estado == true);

            if (existeRol)
                throw new InvalidOperationException($"Ya existe otro rol con el nombre '{model.Nombre}'");

            entity.Nombre = model.Nombre;
            entity.Descripcion = model.Descripcion;
            entity.UsuarioModificacion = model.UsuarioModificacion;
            entity.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
