namespace DcodePe.Catering.Application.DataBase.Categoria.Commands.Update
{
    public interface IUpdateCategoriaCommand
    {
        Task<bool> Execute(UpdateCategoriaModel model);
    }
}
