namespace DcodePe.Catering.Application.DataBase.Usuario.Commands.Create
{
    public interface ICreateUsuarioCommand
    {
        Task<CreateUsuarioResponseModel> Execute(CreateUsuarioModel model);
    }
}
