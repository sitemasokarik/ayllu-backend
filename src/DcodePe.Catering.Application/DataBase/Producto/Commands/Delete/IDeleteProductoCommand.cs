namespace DcodePe.Catering.Application.DataBase.Producto.Commands.Delete
{
    public interface IDeleteProductoCommand
    {
        Task<bool> Execute(int productoId, string usuarioEliminacion);
    }
}
