namespace DcodePe.Catering.Application.DataBase.Local.Queries.GetAllLocal
{
    public interface IGetAllLocalQuery
    {
        Task<List<GetAllLocalModel>> GetAllLocals();
        Task<GetAllLocalModel> GetAllLocalsById(int idLocal);
    }
}
