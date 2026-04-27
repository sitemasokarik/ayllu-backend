using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Categoria.Commands.Delete
{
    public class DeleteCategoriaCommand : IDeleteCategoriaCommand
    {
        private readonly IDataBaseService _databaseService;

        public DeleteCategoriaCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(int categoriaId, string usuarioEliminacion)
        {
            var entity = await _databaseService.Categoria
                .Include(c => c.Productos) // Verificar si tiene productos
                .FirstOrDefaultAsync(c => c.CategoriaID == categoriaId);

            if (entity == null)
                return false;

            // Verificar que no tenga productos activos
            var tieneProductosActivos = entity.Productos.Any(p => p.Estado == true);
            if (tieneProductosActivos)
                throw new InvalidOperationException("No se puede eliminar la categoría porque tiene productos asociados");

            // Soft delete
            entity.Estado = false;
            entity.UsuarioEliminacion = usuarioEliminacion ?? "SYSTEM";
            entity.FechaEliminacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
