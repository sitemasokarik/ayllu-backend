namespace DcodePe.Catering.Application.DataBase.Usuario.Commands.Delete
{
    public interface IDeleteUsuarioCommand
    {
        Task<bool> Execute(int usuarioId, string usuarioEliminacion);
    }
}
