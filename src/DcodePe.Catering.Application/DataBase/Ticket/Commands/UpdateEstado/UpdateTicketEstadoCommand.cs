using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Ticket.Commands.UpdateEstado
{
    public class UpdateTicketEstadoCommand(IDataBaseService databaseService) : IUpdateTicketEstadoCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;

        public async Task<UpdateTicketEstadoModel?> Execute(UpdateTicketEstadoModel model)
        {
            var entity = await _databaseService.TicketInterno
                .FirstOrDefaultAsync(t => t.TicketID == model.TicketID && t.Estado == true);

            if (entity == null)
                return null;

            entity.EstadoTicket = model.EstadoTicket;
            entity.UsuarioModificacion = model.UsuarioModificacion ?? "SYSTEM";
            entity.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();

            return model;
        }
    }
}
