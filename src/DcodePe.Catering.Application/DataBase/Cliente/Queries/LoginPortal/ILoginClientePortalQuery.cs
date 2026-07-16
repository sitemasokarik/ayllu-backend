namespace DcodePe.Catering.Application.DataBase.Cliente.Queries.LoginPortal
{
    public interface ILoginClientePortalQuery
    {
        Task<LoginClientePortalModel?> Execute(string email, string password);
    }
}
