using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.ServicioAdicional.Commands.Delete
{
    public class DeleteServicioAdicionalCommand : IDeleteServicioAdicionalCommand
    {
        private readonly IDataBaseService _databaseService;

        public DeleteServicioAdicionalCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(int servicioId, string usuarioEliminacion)
        {
            var entity = await _databaseService.ServicioAdicional
                .FirstOrDefaultAsync(s => s.ServicioID == servicioId);

            if (entity == null)
                return false;

            entity.Estado = false;
            entity.UsuarioEliminacion = usuarioEliminacion ?? "SYSTEM";
            entity.FechaEliminacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
