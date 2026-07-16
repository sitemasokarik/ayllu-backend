namespace DcodePe.Catering.Application.DataBase.Ticket.Commands.UpdateEstado
{
    public interface IUpdateTicketEstadoCommand
    {
        Task<UpdateTicketEstadoModel?> Execute(UpdateTicketEstadoModel model);
    }
}
