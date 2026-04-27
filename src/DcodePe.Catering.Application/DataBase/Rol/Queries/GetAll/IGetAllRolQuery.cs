namespace DcodePe.Catering.Application.DataBase.Rol.Queries.GetAll
{
    public interface IGetAllRolQuery
    {
        Task<List<GetAllRolModel>> Execute();
    }
}
