namespace DcodePe.Catering.Application.DataBase.Categoria.Commands.Create
{
    public interface ICreateCategoriaCommand
    {
        Task<CreateCategoriaResponseModel> Execute(CreateCategoriaModel model);
    }
}
