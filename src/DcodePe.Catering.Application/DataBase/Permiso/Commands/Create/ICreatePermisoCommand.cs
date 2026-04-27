namespace DcodePe.Catering.Application.DataBase.Permiso.Commands.Create
{
    public interface ICreatePermisoCommand
    {
        Task<CreatePermisoModel> Execute(CreatePermisoModel model);
    }
}
