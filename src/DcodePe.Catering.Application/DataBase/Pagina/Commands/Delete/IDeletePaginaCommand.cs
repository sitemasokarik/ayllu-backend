namespace DcodePe.Catering.Application.DataBase.Pagina.Commands.Delete
{
    public interface IDeletePaginaCommand
    {
        Task<bool> Execute(int paginaId, string usuarioEliminacion);
    }
}
