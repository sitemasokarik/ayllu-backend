using DcodePe.Catering.Domain.Entities.Tickets;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Ticket.Commands.AddMensaje
{
    public class AddTicketMensajeCommand(IDataBaseService databaseService) : IAddTicketMensajeCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;

        public async Task<AddTicketMensajeModel> Execute(AddTicketMensajeModel model)
        {
            var ticketExists = await _databaseService.TicketInterno
                .AnyAsync(t => t.TicketID == model.TicketID && t.Estado == true);

            if (!ticketExists)
                throw new InvalidOperationException("El ticket no existe o está inactivo.");

            var entity = new TicketMensajeEntity
            {
                TicketID = model.TicketID,
                UsuarioID = model.UsuarioID,
                ClienteID = model.ClienteID,
                AutorNombre = model.AutorNombre,
                Mensaje = model.Mensaje,
                EsInterno = model.EsInterno,
                UsuarioCreacion = model.UsuarioCreacion ?? "SYSTEM",
                FechaCreacion = DateTime.Now,
                Estado = true
            };

            await _databaseService.TicketMensaje.AddAsync(entity);
            await _databaseService.SaveAsync();

            model.TicketMensajeID = entity.TicketMensajeID;

            return model;
        }
    }
}
