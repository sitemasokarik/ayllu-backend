using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Local.Commands.Delete
{
    public class DeleteLocalCommand : IDeleteLocalCommand
    {
        private readonly IDataBaseService _databaseService;

        public DeleteLocalCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(int localId, string usuarioEliminacion)
        {
            var entity = await _databaseService.Local
                .Include(l => l.Cotizaciones)
                .FirstOrDefaultAsync(l => l.LocalID == localId);

            if (entity == null)
                return false;

            // Verificar que no tenga cotizaciones activas
            var tieneCotizacionesActivas = entity.Cotizaciones.Any(c => c.Estado == true);
            if (tieneCotizacionesActivas)
                throw new InvalidOperationException("No se puede eliminar el local porque tiene cotizaciones activas asociadas");

            // Soft delete
            entity.Estado = false;
            entity.UsuarioEliminacion = usuarioEliminacion ?? "SYSTEM";
            entity.FechaEliminacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
