namespace DcodePe.Catering.Application.DataBase.PaqueteProducto.Queries.GetAllPaqueteProducto
{
    public interface IGetAllPaqueteProductoQuery
    {
        Task<List<GetAllPaqueteProductoModel>> ExecuteGetAllPaqueteProducto();
    }
}
