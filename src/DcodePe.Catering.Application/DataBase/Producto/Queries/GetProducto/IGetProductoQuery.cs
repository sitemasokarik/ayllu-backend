namespace DcodePe.Catering.Application.DataBase.Producto.Queries.GetProducto
{
    public interface IGetProductoQuery
    {
        Task<List<GetAllProductoModel>> GetAllProductoAsync();
        Task<List<GetAllProductoModel>> GetProductoById(int ProductoID);
        Task<List<GetProductoWithPaquetesModel>> ExecuteGetProductoWithPaquetesByProductoId(int Productoid);
    }
}
