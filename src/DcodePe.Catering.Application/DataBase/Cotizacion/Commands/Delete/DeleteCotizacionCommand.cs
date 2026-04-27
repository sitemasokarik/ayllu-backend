using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Delete
{
    public class DeleteCotizacionCommand : IDeleteCotizacionCommand
    {
        private readonly IDataBaseService _databaseService;

        public DeleteCotizacionCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(int cotizacionId, string usuarioEliminacion)
        {
            var entity = await _databaseService.Cotizacion
                .Include(c => c.CotizacionProducto)
                .Include(c => c.CotizacionServicio)
                //.Include(c => c.CotizacionPaquete)
                .FirstOrDefaultAsync(c => c.CotizacionID == cotizacionId);

            if (entity == null)
                return false;

            // Soft delete de la cotización principal
            entity.Estado = false;
            entity.UsuarioEliminacion = usuarioEliminacion ?? "SYSTEM";
            entity.FechaEliminacion = DateTime.Now;

            // Soft delete de productos relacionados
            foreach (var producto in entity.CotizacionProducto)
            {
                producto.Estado = false;
                producto.UsuarioEliminacion = usuarioEliminacion ?? "SYSTEM";
                producto.FechaEliminacion = DateTime.Now;
            }

            // Soft delete de servicios relacionados
            foreach (var servicio in entity.CotizacionServicio)
            {
                servicio.Estado = false;
                servicio.UsuarioEliminacion = usuarioEliminacion ?? "SYSTEM";
                servicio.FechaEliminacion = DateTime.Now;
            }

            // Soft delete de paquetes relacionados
            //foreach (var paquete in entity.CotizacionPaquete)
            //{
            //    paquete.Estado = false;
            //    paquete.UsuarioEliminacion = usuarioEliminacion ?? "SYSTEM";
            //    paquete.FechaEliminacion = DateTime.Now;
            //}

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
