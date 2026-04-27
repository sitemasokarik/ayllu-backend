using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Producto.Commands.Delete
{
    public class DeleteProductoCommand : IDeleteProductoCommand
    {
        private readonly IDataBaseService _databaseService;

        public DeleteProductoCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(int productoId, string usuarioEliminacion)
        {
            var entity = await _databaseService.Producto
                .Include(p => p.PaqueteProducto) // Verificar si tiene paquetes
                .Include(p => p.CotizacionProducto) // Verificar si tiene cotizaciones
                .FirstOrDefaultAsync(p => p.ProductoID == productoId);

            if (entity == null)
                return false;

            // Verificar que no tenga relaciones activas
            //var tienePaquetesActivos = entity.PaqueteProducto.Any(pp => pp.Estado == true);
            //if (tienePaquetesActivos)
            //    throw new InvalidOperationException("No se puede eliminar el producto porque está asociado a paquetes activos");

            //var tieneCotizacionesActivas = entity.CotizacionProducto.Any(cp => cp.Estado == true);
            //if (tieneCotizacionesActivas)
            //    throw new InvalidOperationException("No se puede eliminar el producto porque está asociado a cotizaciones activas");

            // Soft delete
            entity.Estado = false;
            entity.UsuarioEliminacion = usuarioEliminacion ?? "SYSTEM";
            entity.FechaEliminacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
