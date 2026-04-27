namespace DcodePe.Catering.Application.DataBase.PaqueteProducto.Commands.Create
{
    public interface ICreatePaqueteProductoCommand
    {
        Task<CreatePaqueteProductoModel> ExecuteSavePaqueteProducto(CreatePaqueteProductoModel model);
    }
}
