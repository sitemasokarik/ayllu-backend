namespace DcodePe.Catering.Application.DataBase.Evento.Queries.GetAllEvento
{
    public interface IGetAllEventoQuery
    {
        Task<List<GetAllEventoModel>> ExecuteListEvento();
    }
}
