namespace DcodePe.Catering.Application.DataBase.Cliente.Commands.Delete
{
    public interface IDeleteClienteCommand
    {
        Task<bool> Execute(int clienteId, string usuarioEliminacion);
    }
}
