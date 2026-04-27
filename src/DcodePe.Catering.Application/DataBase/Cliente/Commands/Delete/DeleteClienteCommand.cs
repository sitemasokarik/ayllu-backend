using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Cliente.Commands.Delete
{
    public class DeleteClienteCommand : IDeleteClienteCommand
    {
        private readonly IDataBaseService _databaseService;

        public DeleteClienteCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(int clienteId, string usuarioEliminacion)
        {
            var entity = await _databaseService.Cliente
                .Include(c => c.Cotizaciones) // Verificar si tiene cotizaciones
                .FirstOrDefaultAsync(c => c.ClienteID == clienteId);

            if (entity == null)
                return false;

            // Verificar que no tenga cotizaciones activas
            var tieneCotizacionesActivas = entity.Cotizaciones.Any(cot => cot.Estado == true);
            if (tieneCotizacionesActivas)
                throw new InvalidOperationException("No se puede eliminar el cliente porque tiene cotizaciones activas");

            // Soft delete
            entity.Estado = false;
            entity.UsuarioEliminacion = usuarioEliminacion ?? "SYSTEM";
            entity.FechaEliminacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
