namespace DcodePe.Catering.Application.DataBase.Paquete.Queries.GetAllPaquete
{
    public interface IGetAllPaqueteQuery
    {
        Task<List<GetAllPaqueteModel>> ExecuteGetAllPaquete();
        Task<List<GetPaqueteWithProductosModel>> ExecuteGetPaqueteWithProductos();
        Task<List<GetPaqueteWithProductosModel>> ExecuteGetPaqueteWithProductosByIdPaquete(int paqueteId);
        Task<List<GetPaqueteWithServicioModel>> ExecuteGetPaqueteWithServicios();
        Task<List<GetPaqueteWithServicioModel>> ExecuteGetPaqueteWithServicioByIdPaquete(int paqueteId);

        Task<List<GetAllPaqueteModel>> ExecuteGetAllPaqueteWithProductosAndServicios();
        Task<List<GetAllPaqueteModel>> ExecuteGetAllPaqueteWithProductosAndServiciosById(int paqueteId);


    }
}
