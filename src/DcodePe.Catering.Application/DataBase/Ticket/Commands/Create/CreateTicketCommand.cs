using DcodePe.Catering.Domain.Entities.Tickets;

namespace DcodePe.Catering.Application.DataBase.Ticket.Commands.Create
{
    public class CreateTicketCommand(IDataBaseService databaseService) : ICreateTicketCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;

        public async Task<CreateTicketModel> Execute(CreateTicketModel model)
        {
            var entity = new TicketInternoEntity
            {
                Titulo = model.Titulo,
                Descripcion = model.Descripcion,
                EstadoTicket = string.IsNullOrWhiteSpace(model.EstadoTicket) ? "Abierto" : model.EstadoTicket,
                Prioridad = string.IsNullOrWhiteSpace(model.Prioridad) ? "Media" : model.Prioridad,
                CreadoPorUsuarioID = model.CreadoPorUsuarioID,
                AsignadoUsuarioID = model.AsignadoUsuarioID,
                CreadoPorClienteID = model.CreadoPorClienteID,
                RolDestinoID = model.RolDestinoID,
                CotizacionID = model.CotizacionID,
                UsuarioCreacion = model.UsuarioCreacion ?? "SYSTEM",
                FechaCreacion = DateTime.Now,
                Estado = true
            };

            await _databaseService.TicketInterno.AddAsync(entity);
            await _databaseService.SaveAsync();

            model.TicketID = entity.TicketID;
            model.EstadoTicket = entity.EstadoTicket;

            return model;
        }
    }
}
