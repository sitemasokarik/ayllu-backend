namespace DcodePe.Catering.Application.DataBase.Pagina.Queries.GetById
{
    public interface IGetPaginaByIdQuery
    {
        Task<GetPaginaByIdModel> Execute(int paginaId);
    }
}
