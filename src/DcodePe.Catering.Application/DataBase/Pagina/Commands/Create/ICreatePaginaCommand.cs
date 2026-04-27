namespace DcodePe.Catering.Application.DataBase.Pagina.Commands.Create
{
    public interface ICreatePaginaCommand
    {
        Task<CreatePaginaModel> Execute(CreatePaginaModel model);
    }
}
