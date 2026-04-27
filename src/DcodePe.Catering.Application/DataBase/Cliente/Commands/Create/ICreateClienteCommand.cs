namespace DcodePe.Catering.Application.DataBase.Cliente.Commands.Create
{
    public interface ICreateClienteCommand
    {
        Task<CreateClienteModel> Execute(CreateClienteModel model);
    }
}
