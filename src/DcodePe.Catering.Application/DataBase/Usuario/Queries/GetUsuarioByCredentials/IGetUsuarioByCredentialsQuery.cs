namespace DcodePe.Catering.Application.DataBase.Usuario.Queries.GetUsuarioByCredentials
{
    public interface IGetUsuarioByCredentialsQuery
    {
        Task<GetUsuarioByCredentialsModel> Execute(string userName, string password);
    }
}
