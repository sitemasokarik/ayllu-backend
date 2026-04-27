using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Rol.Commands.Delete
{
    public class DeleteRolCommand : IDeleteRolCommand
    {
        private readonly IDataBaseService _databaseService;

        public DeleteRolCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(int rolId, string usuarioEliminacion)
        {
            var entity = await _databaseService.Rol
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(r => r.RolID == rolId && r.Estado == true);

            if (entity == null)
                return false;

            // Validar que no tenga usuarios asignados
            if (entity.Usuario.Any(u => u.Estado == true))
                throw new InvalidOperationException("No se puede eliminar el rol porque tiene usuarios asignados");

            // Soft Delete
            entity.Estado = false;
            entity.UsuarioEliminacion = usuarioEliminacion;
            entity.FechaEliminacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
