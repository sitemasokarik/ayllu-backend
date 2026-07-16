namespace DcodePe.Catering.Application.DataBase.Ticket.Commands.AddMensaje
{
    public interface IAddTicketMensajeCommand
    {
        Task<AddTicketMensajeModel> Execute(AddTicketMensajeModel model);
    }
}
