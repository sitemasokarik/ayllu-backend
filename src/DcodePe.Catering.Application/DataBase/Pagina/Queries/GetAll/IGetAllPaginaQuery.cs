namespace DcodePe.Catering.Application.DataBase.Pagina.Queries.GetAll
{
    public interface IGetAllPaginaQuery
    {
        Task<List<GetAllPaginaModel>> Execute();
    }
}
