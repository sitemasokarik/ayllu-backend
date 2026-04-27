
using DcodePe.Catering.Application.DataBase.PaqueteServicio.Queries.GetAllPaqueteServicio;

namespace DcodePe.Catering.Application.DataBase.Paquete.Queries.GetAllPaquete
{
    public class GetAllPaqueteQuery(IDataBaseService databaseService) : IGetAllPaqueteQuery
    {
        private readonly IDataBaseService _databaseService = databaseService;
        public async Task<List<GetAllPaqueteModel>> ExecuteGetAllPaquete()
        {
            var result = _databaseService.Paquete
                .Where(x => x.Estado == true)
                .Select(x => new GetAllPaqueteModel
                {
                    PaqueteID = x.PaqueteID,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    PrecioTotal = x.PrecioTotal,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaCreacion = x.FechaCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioEliminacion = x.UsuarioEliminacion,
                    FechaEliminacion = x.FechaEliminacion,
                    Estado = x.Estado,

                    PaqueteProducto = x.PaqueteProducto.Where(x => x.Estado == true).Select(y => new GetAllPaqueteProductoModel
                    {
                        PaqueteID = y.PaqueteID,
                        ProductoID = y.ProductoID,
                        UsuarioCreacion = y.Producto.UsuarioCreacion,
                        FechaCreacion = y.Producto.FechaCreacion,
                        UsuarioModificacion = y.Producto.UsuarioModificacion,
                        FechaModificacion = y.Producto.FechaModificacion,
                        UsuarioEliminacion = y.Producto.UsuarioEliminacion,
                        FechaEliminacion = y.Producto.FechaEliminacion,
                        Estado = y.Producto.Estado,
                        Producto = new GetAllProductoModel
                        {
                            ProductoID = y.Producto.ProductoID,
                            CategoriaID = y.Producto.CategoriaID,
                            Categoria = y.Producto.Categoria.Descripcion,
                            Nombre = y.Producto.Nombre,
                            Descripcion = y.Producto.Descripcion,
                            FotosUrls = string.IsNullOrEmpty(y.Producto.Fotos)
                        ? new List<string>()
                        : y.Producto.Fotos.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList(),
                            Precio = y.Producto.Precio,
                            UsuarioCreacion = y.Producto.UsuarioCreacion,
                            FechaCreacion = y.Producto.FechaCreacion,
                            UsuarioModificacion = y.Producto.UsuarioModificacion,
                            FechaModificacion = y.Producto.FechaModificacion,
                            UsuarioEliminacion = y.Producto.UsuarioEliminacion,
                            FechaEliminacion = y.Producto.FechaEliminacion,
                            Estado = y.Producto.Estado
                        }

                    }).ToList()
                    ,
                    PaqueteServicio = x.PaqueteServicio.Where(x => x.Estado == true).Select(y => new GetAllPaqueteServicioModel
                    {
                        PaqueteID = y.PaqueteID,
                        ServicioID = y.ServicioID,
                        UsuarioCreacion = y.Servicio.UsuarioCreacion,
                        FechaCreacion = y.Servicio.FechaCreacion,
                        UsuarioModificacion = y.Servicio.UsuarioModificacion,
                        FechaModificacion = y.Servicio.FechaModificacion,
                        UsuarioEliminacion = y.Servicio.UsuarioEliminacion,
                        FechaEliminacion = y.Servicio.FechaEliminacion,
                        Estado = y.Servicio.Estado,
                        Servicio = new GetAllServicioAdicionalModel
                        {
                            ServicioID = y.Servicio.ServicioID,
                            Nombre = y.Servicio.Nombre,
                            Descripcion = y.Servicio.Descripcion,
                            Precio = y.Servicio.Precio,
                            UsuarioCreacion = y.Servicio.UsuarioCreacion,
                            FechaCreacion = y.Servicio.FechaCreacion,
                            UsuarioModificacion = y.Servicio.UsuarioModificacion,
                            FechaModificacion = y.Servicio.FechaModificacion,
                            UsuarioEliminacion = y.Servicio.UsuarioEliminacion,
                            FechaEliminacion = y.Servicio.FechaEliminacion,
                            Estado = y.Servicio.Estado
                        }
                    }).ToList(),
                }).ToList();

            return result;

        }

        public async Task<List<GetPaqueteWithProductosModel>> ExecuteGetPaqueteWithProductos()
        {
            var result = await _databaseService.Paquete
                .Select(x => new GetPaqueteWithProductosModel
                {
                    PaqueteID = x.PaqueteID,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    PrecioTotal = x.PrecioTotal,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaCreacion = x.FechaCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioEliminacion = x.UsuarioEliminacion,
                    FechaEliminacion = x.FechaEliminacion,
                    Estado = x.Estado,
                    Producto = x.PaqueteProducto.Where(x => x.Estado == true).Select(y => new GetAllProductoModel
                    {
                        ProductoID = y.ProductoID,
                        Nombre = y.Producto.Nombre,
                        Descripcion = y.Producto.Descripcion,
                        Precio = y.Producto.Precio,
                        UsuarioCreacion = y.Producto.UsuarioCreacion,
                        FechaCreacion = y.Producto.FechaCreacion,
                        UsuarioModificacion = y.Producto.UsuarioModificacion,
                        FechaModificacion = y.Producto.FechaModificacion,
                        UsuarioEliminacion = y.Producto.UsuarioEliminacion,
                        FechaEliminacion = y.Producto.FechaEliminacion,
                        Estado = y.Producto.Estado
                    }).ToList()
                }).ToListAsync();

            return result;
        }

        public async Task<List<GetPaqueteWithProductosModel>> ExecuteGetPaqueteWithProductosByIdPaquete(int paqueteId)
        {
            var result = await _databaseService.Paquete
                .Where(x => x.PaqueteID == paqueteId)
               .Select(x => new GetPaqueteWithProductosModel
               {
                   PaqueteID = x.PaqueteID,
                   Nombre = x.Nombre,
                   Descripcion = x.Descripcion,
                   PrecioTotal = x.PrecioTotal,
                   UsuarioCreacion = x.UsuarioCreacion,
                   FechaCreacion = x.FechaCreacion,
                   UsuarioModificacion = x.UsuarioModificacion,
                   FechaModificacion = x.FechaModificacion,
                   UsuarioEliminacion = x.UsuarioEliminacion,
                   FechaEliminacion = x.FechaEliminacion,
                   Estado = x.Estado,
                   Producto = x.PaqueteProducto.Where(x => x.Estado == true).Select(y => new GetAllProductoModel
                   {

                       ProductoID = y.ProductoID,
                       Nombre = y.Producto.Nombre,
                       Descripcion = y.Producto.Descripcion,
                       Precio = y.Producto.Precio,
                       UsuarioCreacion = y.Producto.UsuarioCreacion,
                       FechaCreacion = y.Producto.FechaCreacion,
                       UsuarioModificacion = y.Producto.UsuarioModificacion,
                       FechaModificacion = y.Producto.FechaModificacion,
                       UsuarioEliminacion = y.Producto.UsuarioEliminacion,
                       FechaEliminacion = y.Producto.FechaEliminacion,
                       Estado = y.Producto.Estado
                   }).ToList()
               }).ToListAsync();

            return result;

        }

        public async Task<List<GetPaqueteWithServicioModel>> ExecuteGetPaqueteWithServicios()
        {
            var result = await _databaseService.Paquete
               .Select(x => new GetPaqueteWithServicioModel
               {
                   PaqueteID = x.PaqueteID,
                   Nombre = x.Nombre,
                   Descripcion = x.Descripcion,
                   PrecioTotal = x.PrecioTotal,
                   UsuarioCreacion = x.UsuarioCreacion,
                   FechaCreacion = x.FechaCreacion,
                   UsuarioModificacion = x.UsuarioModificacion,
                   FechaModificacion = x.FechaModificacion,
                   UsuarioEliminacion = x.UsuarioEliminacion,
                   FechaEliminacion = x.FechaEliminacion,
                   Estado = x.Estado,

                   Servicio = x.PaqueteServicio.Where(x => x.Estado == true).Select(y => new GetAllServicioAdicionalModel
                   {
                       ServicioID = y.ServicioID,
                       Nombre = y.Servicio.Nombre,
                       Descripcion = y.Servicio.Descripcion,
                       Precio = y.Servicio.Precio,
                       UsuarioCreacion = y.Servicio.UsuarioCreacion,
                       FechaCreacion = y.Servicio.FechaCreacion,
                       UsuarioModificacion = y.Servicio.UsuarioModificacion,
                       FechaModificacion = y.Servicio.FechaModificacion,
                       UsuarioEliminacion = y.Servicio.UsuarioEliminacion,
                       FechaEliminacion = y.Servicio.FechaEliminacion,
                       Estado = y.Servicio.Estado
                   }).ToList()
               }).ToListAsync();

            return result;
        }
        public async Task<List<GetPaqueteWithServicioModel>> ExecuteGetPaqueteWithServicioByIdPaquete(int paqueteId)
        {
            var result = await _databaseService.Paquete
                .Where(x => x.PaqueteID == paqueteId)
               .Select(x => new GetPaqueteWithServicioModel
               {
                   PaqueteID = x.PaqueteID,
                   Nombre = x.Nombre,
                   Descripcion = x.Descripcion,
                   PrecioTotal = x.PrecioTotal,
                   UsuarioCreacion = x.UsuarioCreacion,
                   FechaCreacion = x.FechaCreacion,
                   UsuarioModificacion = x.UsuarioModificacion,
                   FechaModificacion = x.FechaModificacion,
                   UsuarioEliminacion = x.UsuarioEliminacion,
                   FechaEliminacion = x.FechaEliminacion,
                   Estado = x.Estado,

                   Servicio = x.PaqueteServicio.Where(x => x.Estado == true).Select(y => new GetAllServicioAdicionalModel
                   {
                       ServicioID = y.ServicioID,
                       Nombre = y.Servicio.Nombre,
                       Descripcion = y.Servicio.Descripcion,
                       Precio = y.Servicio.Precio,
                       UsuarioCreacion = y.Servicio.UsuarioCreacion,
                       FechaCreacion = y.Servicio.FechaCreacion,
                       UsuarioModificacion = y.Servicio.UsuarioModificacion,
                       FechaModificacion = y.Servicio.FechaModificacion,
                       UsuarioEliminacion = y.Servicio.UsuarioEliminacion,
                       FechaEliminacion = y.Servicio.FechaEliminacion,
                       Estado = y.Servicio.Estado
                   }).ToList()
               }).ToListAsync();

            return result;
        }

        public async Task<List<GetAllPaqueteModel>> ExecuteGetAllPaqueteWithProductosAndServicios()
        {
            var result = _databaseService.Paquete
                .Select(x => new GetAllPaqueteModel
                {
                    PaqueteID = x.PaqueteID,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    PrecioTotal = x.PrecioTotal,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaCreacion = x.FechaCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioEliminacion = x.UsuarioEliminacion,
                    FechaEliminacion = x.FechaEliminacion,
                    Estado = x.Estado,
                    PaqueteProducto = x.PaqueteProducto.Where(x => x.Estado == true).Select(y => new GetAllPaqueteProductoModel
                    {

                        PaqueteID = y.PaqueteID,
                        ProductoID = y.ProductoID,
                        UsuarioCreacion = y.UsuarioCreacion,
                        FechaCreacion = y.FechaCreacion,
                        UsuarioModificacion = y.UsuarioModificacion,
                        FechaModificacion = y.FechaModificacion,
                        UsuarioEliminacion = y.UsuarioEliminacion,
                        FechaEliminacion = y.FechaEliminacion,
                        Estado = y.Estado,
                        Producto = new GetAllProductoModel
                        {
                            ProductoID = y.Producto.ProductoID,
                            Nombre = y.Producto.Nombre,
                            Descripcion = y.Producto.Descripcion,
                            Precio = y.Producto.Precio,
                            UsuarioCreacion = y.Producto.UsuarioCreacion,
                            FechaCreacion = y.Producto.FechaCreacion,
                            UsuarioModificacion = y.Producto.UsuarioModificacion,
                            FechaModificacion = y.Producto.FechaModificacion,
                            UsuarioEliminacion = y.Producto.UsuarioEliminacion,
                            FechaEliminacion = y.Producto.FechaEliminacion,
                            Estado = y.Producto.Estado
                        }
                    }).ToList(),

                    PaqueteServicio = x.PaqueteServicio.Where(x => x.Estado == true).Select(y => new GetAllPaqueteServicioModel
                    {
                        PaqueteID = y.PaqueteID,
                        ServicioID = y.ServicioID,
                        UsuarioCreacion = y.UsuarioCreacion,
                        FechaCreacion = y.FechaCreacion,
                        UsuarioModificacion = y.UsuarioModificacion,
                        FechaModificacion = y.FechaModificacion,
                        UsuarioEliminacion = y.UsuarioEliminacion,
                        FechaEliminacion = y.FechaEliminacion,
                        Estado = y.Estado,
                        Servicio = new GetAllServicioAdicionalModel
                        {
                            ServicioID = y.Servicio.ServicioID,
                            Nombre = y.Servicio.Nombre,
                            Descripcion = y.Servicio.Descripcion,
                            Precio = y.Servicio.Precio,
                            UsuarioCreacion = y.Servicio.UsuarioCreacion,
                            FechaCreacion = y.Servicio.FechaCreacion,
                            UsuarioModificacion = y.Servicio.UsuarioModificacion,
                            FechaModificacion = y.Servicio.FechaModificacion,
                            UsuarioEliminacion = y.Servicio.UsuarioEliminacion,
                            FechaEliminacion = y.Servicio.FechaEliminacion,
                            Estado = y.Servicio.Estado
                        }
                    }).ToList()

                }).ToList();

            return result;
        }

        public async Task<List<GetAllPaqueteModel>> ExecuteGetAllPaqueteWithProductosAndServiciosById(int paqueteId)
        {
            var result = await _databaseService.Paquete
                .Where(x => x.PaqueteID == paqueteId)
               .Select(x => new GetAllPaqueteModel
               {
                   PaqueteID = x.PaqueteID,
                   Nombre = x.Nombre,
                   Descripcion = x.Descripcion,
                   PrecioTotal = x.PrecioTotal,
                   UsuarioCreacion = x.UsuarioCreacion,
                   FechaCreacion = x.FechaCreacion,
                   UsuarioModificacion = x.UsuarioModificacion,
                   FechaModificacion = x.FechaModificacion,
                   UsuarioEliminacion = x.UsuarioEliminacion,
                   FechaEliminacion = x.FechaEliminacion,
                   Estado = x.Estado,
                   PaqueteProducto = x.PaqueteProducto.Where(x => x.Estado == true).Select(y => new GetAllPaqueteProductoModel
                   {

                       PaqueteID = y.PaqueteID,
                       ProductoID = y.ProductoID,
                       UsuarioCreacion = y.UsuarioCreacion,
                       FechaCreacion = y.FechaCreacion,
                       UsuarioModificacion = y.UsuarioModificacion,
                       FechaModificacion = y.FechaModificacion,
                       UsuarioEliminacion = y.UsuarioEliminacion,
                       FechaEliminacion = y.FechaEliminacion,
                       Estado = y.Estado,
                       Producto = new GetAllProductoModel
                       {
                           ProductoID = y.Producto.ProductoID,
                           Nombre = y.Producto.Nombre,
                           Descripcion = y.Producto.Descripcion,
                           Precio = y.Producto.Precio,
                           UsuarioCreacion = y.Producto.UsuarioCreacion,
                           FechaCreacion = y.Producto.FechaCreacion,
                           UsuarioModificacion = y.Producto.UsuarioModificacion,
                           FechaModificacion = y.Producto.FechaModificacion,
                           UsuarioEliminacion = y.Producto.UsuarioEliminacion,
                           FechaEliminacion = y.Producto.FechaEliminacion,
                           Estado = y.Producto.Estado
                       }
                   }).ToList(),

                   PaqueteServicio = x.PaqueteServicio.Where(x => x.Estado == true).Select(y => new GetAllPaqueteServicioModel
                   {
                       PaqueteID = y.PaqueteID,
                       ServicioID = y.ServicioID,
                       UsuarioCreacion = y.UsuarioCreacion,
                       FechaCreacion = y.FechaCreacion,
                       UsuarioModificacion = y.UsuarioModificacion,
                       FechaModificacion = y.FechaModificacion,
                       UsuarioEliminacion = y.UsuarioEliminacion,
                       FechaEliminacion = y.FechaEliminacion,
                       Estado = y.Estado,
                       Servicio = new GetAllServicioAdicionalModel
                       {
                           ServicioID = y.Servicio.ServicioID,
                           Nombre = y.Servicio.Nombre,
                           Descripcion = y.Servicio.Descripcion,
                           Precio = y.Servicio.Precio,
                           UsuarioCreacion = y.Servicio.UsuarioCreacion,
                           FechaCreacion = y.Servicio.FechaCreacion,
                           UsuarioModificacion = y.Servicio.UsuarioModificacion,
                           FechaModificacion = y.Servicio.FechaModificacion,
                           UsuarioEliminacion = y.Servicio.UsuarioEliminacion,
                           FechaEliminacion = y.Servicio.FechaEliminacion,
                           Estado = y.Servicio.Estado
                       }
                   }).ToList()

               }).ToListAsync();

            return result;
        }
    }
}
