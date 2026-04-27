namespace DcodePe.Catering.Application.DataBase.PaqueteServicio.Queries.GetAllPaqueteServicio
{
    public interface IGetAllPaqueteServicioQuery
    {
         Task<List<GetAllPaqueteServicioModel>> GetAllPaqueteServicio();
    }
}
