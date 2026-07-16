namespace DcodePe.Catering.Application.DataBase.Ticket.Commands.Create
{
    public interface ICreateTicketCommand
    {
        Task<CreateTicketModel> Execute(CreateTicketModel model);
    }
}
