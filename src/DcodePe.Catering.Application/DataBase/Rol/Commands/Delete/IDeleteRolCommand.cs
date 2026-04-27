namespace DcodePe.Catering.Application.DataBase.Rol.Commands.Delete
{
    public interface IDeleteRolCommand
    {
        Task<bool> Execute(int rolId, string usuarioEliminacion);
    }
}
