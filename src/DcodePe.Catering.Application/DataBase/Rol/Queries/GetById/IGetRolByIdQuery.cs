namespace DcodePe.Catering.Application.DataBase.Rol.Queries.GetById
{
    public interface IGetRolByIdQuery
    {
        Task<GetRolByIdModel> Execute(int rolId);
    }
}
