namespace DcodePe.Catering.Application.DataBase.Categoria.Queries.GetById
{
    public interface IGetCategoriaByIdQuery
    {
        Task<GetCategoriaByIdModel> Execute(int categoriaId);
    }
}
