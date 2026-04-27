namespace DcodePe.Catering.Application.DataBase.Evento.Queries.GetById
{
    public interface IGetEventoByIdQuery
    {
        Task<GetEventoByIdModel> Execute(int eventoId);
    }
}
