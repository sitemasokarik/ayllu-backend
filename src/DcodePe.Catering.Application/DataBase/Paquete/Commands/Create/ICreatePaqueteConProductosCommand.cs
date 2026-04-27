namespace DcodePe.Catering.Application.DataBase.Paquete.Commands.Create
{
    public interface ICreatePaqueteConProductosCommand
    {
        Task<CreatePaqueteConProductosResponseModel> ExecuteSavePaqueteConProductos(CreatePaqueteProductosModel model);
    }
}
