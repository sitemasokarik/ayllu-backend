namespace DcodePe.Catering.Application.DataBase.Categoria.Commands.Delete
{
    public interface IDeleteCategoriaCommand
    {
        Task<bool> Execute(int categoriaId, string usuarioEliminacion);
    }
}
