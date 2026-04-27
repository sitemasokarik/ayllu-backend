namespace DcodePe.Catering.Application.DataBase.Permiso.Commands.Delete
{
    public interface IDeletePermisoCommand
    {
        Task<bool> Execute(int permisoId, string usuarioEliminacion);
    }
}
