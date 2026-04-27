namespace DcodePe.Catering.Application.DataBase.Permiso.Queries.GetByRolId
{
    public interface IGetPermisosByRolIdQuery
    {
        Task<List<GetPermisosByRolIdModel>> Execute(int rolId);
    }
}
