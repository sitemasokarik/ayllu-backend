namespace DcodePe.Catering.Application.DataBase.PaqueteProducto.Queries.GetAllPaqueteProducto
{
    public class GetAllPaqueteProductoQuery(IDataBaseService databaseService) : IGetAllPaqueteProductoQuery
    {
        private readonly IDataBaseService _databaseService = databaseService;
        public async Task<List<GetAllPaqueteProductoModel>> ExecuteGetAllPaqueteProducto()
        {
            var result = await _databaseService.PaqueteProducto
                .Where(x => x.Estado == true)
                .Include(x=>x.Producto).Where(x=>x.Producto!.Estado==true)
                .Include(x => x.Paquete).Where(x => x.Paquete!.Estado == true)
                .Select(paqueteProducto => new GetAllPaqueteProductoModel
                {
                    PaqueteID = paqueteProducto.PaqueteID,
                    ProductoID = paqueteProducto.ProductoID,
                    UsuarioCreacion = paqueteProducto.UsuarioCreacion,
                    FechaCreacion = paqueteProducto.FechaCreacion,
                    UsuarioModificacion = paqueteProducto.UsuarioModificacion,
                    FechaModificacion = paqueteProducto.FechaModificacion,
                    UsuarioEliminacion = paqueteProducto.UsuarioEliminacion,
                    FechaEliminacion = paqueteProducto.FechaEliminacion,
                    Estado = paqueteProducto.Estado,
                    Producto = paqueteProducto.Producto == null ? null : new GetAllProductoModel
                    {
                        ProductoID = paqueteProducto.Producto!.ProductoID,
                        Nombre = paqueteProducto.Producto.Nombre,
                        Descripcion = paqueteProducto.Producto.Descripcion,
                        Precio = paqueteProducto.Producto.Precio,
                        UsuarioCreacion = paqueteProducto.Producto.UsuarioCreacion,
                        FechaCreacion = paqueteProducto.Producto.FechaCreacion,
                        UsuarioModificacion = paqueteProducto.Producto.UsuarioModificacion,
                        FechaModificacion = paqueteProducto.Producto.FechaModificacion,
                        UsuarioEliminacion = paqueteProducto.Producto.UsuarioEliminacion,
                        FechaEliminacion = paqueteProducto.Producto.FechaEliminacion,
                        Estado = paqueteProducto.Producto.Estado
                    },

                }).ToListAsync();

            return result;
        }
    }
}
