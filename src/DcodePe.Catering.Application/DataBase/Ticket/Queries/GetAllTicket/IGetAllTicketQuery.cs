namespace DcodePe.Catering.Application.DataBase.Ticket.Queries.GetAllTicket
{
    public interface IGetAllTicketQuery
    {
        Task<List<GetAllTicketModel>> Execute(int? usuarioId = null, int? clienteId = null);
    }
}
