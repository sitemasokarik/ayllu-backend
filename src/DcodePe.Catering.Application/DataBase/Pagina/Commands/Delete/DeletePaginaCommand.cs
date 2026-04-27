using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Pagina.Commands.Delete
{
    public class DeletePaginaCommand : IDeletePaginaCommand
    {
        private readonly IDataBaseService _databaseService;

        public DeletePaginaCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(int paginaId, string usuarioEliminacion)
        {
            var entity = await _databaseService.Pagina
                .Include(p => p.Permiso)
                .FirstOrDefaultAsync(p => p.PaginaID == paginaId && p.Estado == true);

            if (entity == null)
                return false;

            // Validar que no tenga permisos asociados
            if (entity.Permiso.Any(per => per.Estado == true))
                throw new InvalidOperationException("No se puede eliminar la página porque tiene permisos asociados");

            // Soft Delete
            entity.Estado = false;
            entity.UsuarioEliminacion = usuarioEliminacion;
            entity.FechaEliminacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
