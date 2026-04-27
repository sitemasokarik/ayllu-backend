namespace DcodePe.Catering.Application.DataBase.Pagina.Commands.Update
{
    public interface IUpdatePaginaCommand
    {
        Task<bool> Execute(UpdatePaginaModel model);
    }
}
