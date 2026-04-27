namespace DcodePe.Catering.Application.DataBase.Permiso.Queries.GetAll
{
    public interface IGetAllPermisoQuery
    {
        Task<List<GetAllPermisoModel>> Execute();
    }
}
