namespace DcodePe.Catering.Application.DataBase.Cliente.Commands.Update
{
    public interface IUpdateClienteCommand
    {
        Task<bool> Execute(UpdateClienteModel model);
    }
}
