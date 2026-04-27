using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Evento.Commands.Delete
{
    public class DeleteEventoCommand : IDeleteEventoCommand
    {
        private readonly IDataBaseService _databaseService;

        public DeleteEventoCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(int eventoId, string usuarioEliminacion)
        {
            var entity = await _databaseService.Evento
                .FirstOrDefaultAsync(e => e.EventoID == eventoId);

            if (entity == null)
                return false;

            // Soft delete
            entity.Estado = false;
            entity.UsuarioEliminacion = usuarioEliminacion ?? "SYSTEM";
            entity.FechaEliminacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
