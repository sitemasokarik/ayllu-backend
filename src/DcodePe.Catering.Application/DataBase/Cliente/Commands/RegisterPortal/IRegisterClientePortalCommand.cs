namespace DcodePe.Catering.Application.DataBase.Cliente.Commands.RegisterPortal
{
    public interface IRegisterClientePortalCommand
    {
        Task<RegisterClientePortalModel> Execute(RegisterClientePortalModel model);
    }
}
