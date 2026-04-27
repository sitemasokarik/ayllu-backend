using DcodePe.Catering.Application.Database;
using DcodePe.Catering.Application.DataBase.Paquete.Queries.GetAllPaquete;

namespace DcodePe.Catering.Application.DataBase.Producto.Queries.GetProducto
{
    public class GetProductoQuery(IDataBaseService databaseService) : IGetProductoQuery
    {
        private readonly IDataBaseService _databaseService = databaseService;
        public async Task<List<GetAllProductoModel>> GetAllProductoAsync()
        {
            var result = await _databaseService.Producto.Where(x => x.Estado == true && x.Categoria.Estado == true)
                .Select(Producto => new GetAllProductoModel()
                {
                    ProductoID = Producto.ProductoID,
                    Nombre = Producto.Nombre,
                    Descripcion = Producto.Descripcion,
                    Precio = Producto.Precio,
                    PrecioCosto = Producto.PrecioCosto,
                    FotosUrls = string.IsNullOrEmpty(Producto.Fotos)
                        ? new List<string>()
                        : Producto.Fotos.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    CategoriaID = Producto.CategoriaID,
                    Categoria = Producto.Categoria.Nombre,
                    UsuarioCreacion = Producto.UsuarioCreacion,
                    FechaCreacion = Producto.FechaCreacion,
                    UsuarioModificacion = Producto.UsuarioModificacion,
                    FechaModificacion = Producto.FechaModificacion,
                    UsuarioEliminacion = Producto.UsuarioEliminacion,
                    FechaEliminacion = Producto.FechaEliminacion,
                    Estado = Producto.Estado

                }).ToListAsync();

            return result;
        }

        public async Task<List<GetAllProductoModel>> GetProductoById(int ProductoID)
        {
            var result = await _databaseService.Producto
                .Where(x => x.ProductoID == ProductoID && x.Estado == true && x.Categoria.Estado == true)
                .Select(Producto => new GetAllProductoModel()
                {
                    ProductoID = Producto.ProductoID,
                    Nombre = Producto.Nombre,
                    Descripcion = Producto.Descripcion,
                    Precio = Producto.Precio,
                    PrecioCosto = Producto.PrecioCosto,
                    FotosUrls = string.IsNullOrEmpty(Producto.Fotos)
                        ? new List<string>()
                        : Producto.Fotos.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    CategoriaID = Producto.CategoriaID,
                    Categoria = Producto.Categoria.Nombre,
                    UsuarioCreacion = Producto.UsuarioCreacion,
                    FechaCreacion = Producto.FechaCreacion,
                    UsuarioModificacion = Producto.UsuarioModificacion,
                    FechaModificacion = Producto.FechaModificacion,
                    UsuarioEliminacion = Producto.UsuarioEliminacion,
                    FechaEliminacion = Producto.FechaEliminacion,
                    Estado = Producto.Estado

                }).ToListAsync();

            return result;
        }

        public async Task<List<GetProductoWithPaquetesModel>> ExecuteGetProductoWithPaquetesByProductoId(int Productoid)
        {
            var result = await _databaseService.Producto
                        .Where(x => x.ProductoID == Productoid && x.Estado == true)
                        .Select(Producto => new GetProductoWithPaquetesModel()
                        {
                            ProductoID = Producto.ProductoID,
                            Nombre = Producto.Nombre,
                            Descripcion = Producto.Descripcion,
                            Precio = Producto.Precio,
                            PrecioCosto = Producto.PrecioCosto,
                            FotosUrls = string.IsNullOrEmpty(Producto.Fotos)
                        ? new List<string>()
                        : Producto.Fotos.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList(),
                            CategoriaID = Producto.CategoriaID,
                            Categoria = Producto.Categoria.Nombre,
                            UsuarioCreacion = Producto.UsuarioCreacion,
                            FechaCreacion = Producto.FechaCreacion,
                            UsuarioModificacion = Producto.UsuarioModificacion,
                            FechaModificacion = Producto.FechaModificacion,
                            UsuarioEliminacion = Producto.UsuarioEliminacion,
                            FechaEliminacion = Producto.FechaEliminacion,
                            Estado = Producto.Estado,
                            Paquete = Producto.PaqueteProducto.Where(y => y.Estado == true).Select(y => new GetAllPaqueteModel
                            {
                                PaqueteID = y.Paquete.PaqueteID,
                                Nombre = y.Paquete.Nombre,
                                Descripcion = y.Paquete.Nombre,
                                PrecioTotal = y.Paquete.PrecioTotal,
                                UsuarioCreacion = y.Paquete.UsuarioCreacion,
                                FechaCreacion = y.Paquete.FechaCreacion,
                                UsuarioModificacion = y.Paquete.UsuarioModificacion,
                                FechaModificacion = y.Paquete.FechaModificacion,
                                UsuarioEliminacion = y.Paquete.UsuarioEliminacion,
                                FechaEliminacion = y.Paquete.FechaEliminacion,
                                Estado = y.Paquete.Estado
                            }).ToList()
                        }).ToListAsync();

            return result;
        }
    }
}
