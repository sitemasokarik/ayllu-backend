namespace DcodePe.Catering.Application.DataBase.Producto.Commands.Update
{
    public interface IUpdateProductoCommand
    {
        Task<bool> Execute(UpdateProductoModel model);
    }
}
