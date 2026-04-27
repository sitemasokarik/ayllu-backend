namespace DcodePe.Catering.Application.DataBase.Permiso.Commands.Update
{
    public interface IUpdatePermisoCommand
    {
        Task<bool> Execute(UpdatePermisoModel model);
    }
}
