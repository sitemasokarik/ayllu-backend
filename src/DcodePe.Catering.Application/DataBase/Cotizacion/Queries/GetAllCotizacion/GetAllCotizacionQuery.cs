using DcodePe.Catering.Application.DataBase.ServicioAdicional.Queries.GetAllServicioAdicional;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetAllCotizacion
{
    public class GetAllCotizacionQuery(IDataBaseService databaseService) : IGetAllCotizacionQuery
    {
        private readonly IDataBaseService _databaseService = databaseService;

        public async Task<List<GetAllCotizacionModel>> ExecuteListaCotizacion()
        {
            var result = await _databaseService.Cotizacion
                .Where(X => X.Estado == true)
                .Include(c => c.Cliente)
                .Include(c => c.Local)
                .Include(c => c.Evento)
                .Select(cotizacion => new GetAllCotizacionModel()
                {
                    CotizacionID = cotizacion.CotizacionID,
                    ClienteID = cotizacion.ClienteID,
                    ClienteNombre = cotizacion.Cliente.NombreCompleto,
                    ClienteDocumento = cotizacion.Cliente.NumeroDocumento,
                    ClienteTelefono = cotizacion.Cliente.Telefono,
                    ClienteEmail = cotizacion.Cliente.Email,
                    LocalID = cotizacion.LocalID,
                    LocalNombre = cotizacion.Local.Nombre,
                    LocalDireccion = cotizacion.Local.Direccion,
                    LocalCapacidad = cotizacion.Local.Capacidad,
                    LocalPrecioAlquiler = cotizacion.Local.PrecioAlquiler,
                    LocalHorasEvento = cotizacion.Local.HorasEvento,
                    EventoID = cotizacion.EventoID,
                    EventoNombre = cotizacion.Evento != null ? cotizacion.Evento.Nombre : null,
                    EventoDescripcion = cotizacion.Evento != null ? cotizacion.Evento.Descripcion : null,
                    FechaTentativa = cotizacion.FechaTentativa,
                    FechaTentativaOpcional = cotizacion.FechaTentativaOpcional,
                    FechaContacto = cotizacion.FechaContacto,
                    HoraContacto = cotizacion.HoraContacto,
                    NumeroInvitados = cotizacion.NumeroInvitados,
                    CostoDePersonal = cotizacion.CostoDePersonal,
                    Garantia = cotizacion.Garantia,
                    TarifaMenuPorInvitado = cotizacion.TarifaMenuPorInvitado,
                    SubtotalMenu = cotizacion.SubtotalMenu,
                    TotalEvento = cotizacion.TotalEvento,
                    PrecioPorCubierto = cotizacion.PrecioPorCubierto,
                    PrecioPorCubiertoConDescuento = cotizacion.PrecioPorCubiertoConDescuento,
                    TotalCotizacion = cotizacion.TotalCotizacion,
                    Observacion = cotizacion.Observacion,
                    EstadoCotizacion = cotizacion.EstadoCotizacion,
                    UsuarioCreacion = cotizacion.UsuarioCreacion,
                    FechaCreacion = cotizacion.FechaCreacion,
                    UsuarioModificacion = cotizacion.UsuarioModificacion,
                    FechaModificacion = cotizacion.FechaModificacion,
                    UsuarioEliminacion = cotizacion.UsuarioEliminacion,
                    FechaEliminacion = cotizacion.FechaEliminacion,
                    Estado = cotizacion.Estado,
                    CotizacionProducto = cotizacion.CotizacionProducto.Where(x => x.Estado == true).Select(cotizacionProducto => new GetAllCotizacionProductoModel()
                    {
                        CotizacionID = cotizacionProducto.CotizacionID,
                        ProductoID = cotizacionProducto.ProductoID,
                        Cantidad = cotizacionProducto.Cantidad,
                        UsuarioCreacion = cotizacionProducto.UsuarioCreacion,
                        FechaCreacion = cotizacionProducto.FechaCreacion,
                        UsuarioModificacion = cotizacionProducto.UsuarioModificacion,
                        FechaModificacion = cotizacionProducto.FechaModificacion,
                        UsuarioEliminacion = cotizacionProducto.UsuarioEliminacion,
                        FechaEliminacion = cotizacionProducto.FechaEliminacion,
                        Estado = cotizacionProducto.Estado,
                        Producto = new GetAllProductoModel()
                        {
                            ProductoID = cotizacionProducto.Producto.ProductoID,
                            Nombre = cotizacionProducto.Producto.Nombre,
                            Descripcion = cotizacionProducto.Producto.Descripcion,
                            Precio = cotizacionProducto.Producto.Precio,
                            PrecioCosto = cotizacionProducto.Producto.PrecioCosto,
                            FotosUrls = string.IsNullOrEmpty(cotizacionProducto.Producto.Fotos)
    ? new List<string>()
    : cotizacionProducto.Producto.Fotos.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList(),

                            CategoriaID = cotizacionProducto.Producto.CategoriaID,
                            Categoria = cotizacionProducto.Producto.Categoria.Nombre,
                            UsuarioCreacion = cotizacionProducto.Producto.UsuarioCreacion,
                            FechaCreacion = cotizacionProducto.Producto.FechaCreacion,
                            UsuarioModificacion = cotizacionProducto.Producto.UsuarioModificacion,
                            FechaModificacion = cotizacionProducto.Producto.FechaModificacion,
                            UsuarioEliminacion = cotizacionProducto.Producto.UsuarioEliminacion,
                            FechaEliminacion = cotizacionProducto.Producto.FechaEliminacion,
                            Estado = cotizacionProducto.Producto.Estado
                        }
                    }).ToList(),
                    CotizacionServicio = cotizacion.CotizacionServicio.Where(x => x.Estado == true).Select(cotizacionServicio => new GetAllCotizacionServicioModel()
                    {
                        CotizacionID = cotizacionServicio.CotizacionID,
                        ServicioID = cotizacionServicio.ServicioID,
                        Cantidad = cotizacionServicio.Cantidad,
                        UsuarioCreacion = cotizacionServicio.UsuarioCreacion,
                        FechaCreacion = cotizacionServicio.FechaCreacion,
                        UsuarioModificacion = cotizacionServicio.UsuarioModificacion,
                        FechaModificacion = cotizacionServicio.FechaModificacion,
                        UsuarioEliminacion = cotizacionServicio.UsuarioEliminacion,
                        FechaEliminacion = cotizacionServicio.FechaEliminacion,
                        Estado = cotizacionServicio.Estado,
                        ServicioAdicional = new GetAllServicioAdicionalModel()
                        {
                            ServicioID = cotizacionServicio.Servicio.ServicioID,
                            Nombre = cotizacionServicio.Servicio.Nombre,
                            Descripcion = cotizacionServicio.Servicio.Descripcion,
                            Precio = cotizacionServicio.Servicio.Precio,
                            UsuarioCreacion = cotizacionServicio.Servicio.UsuarioCreacion,
                            FechaCreacion = cotizacionServicio.Servicio.FechaCreacion,
                            UsuarioModificacion = cotizacionServicio.Servicio.UsuarioModificacion,
                            FechaModificacion = cotizacionServicio.Servicio.FechaModificacion,
                            UsuarioEliminacion = cotizacionServicio.Servicio.UsuarioEliminacion,
                            FechaEliminacion = cotizacionServicio.Servicio.FechaEliminacion,
                            Estado = cotizacionServicio.Servicio.Estado
                        }
                    }).ToList(),
                }).ToListAsync();

            return result;
        }

        public async Task<List<GetAllCotizacionModel>> ExecuteListaCotizacionById(int CotizacionID)
        {
            var result = await _databaseService.Cotizacion
                .Include(c => c.Cliente).Where(X => X.Estado == true)
                .Include(c => c.Local).Where(X => X.Estado == true)
                .Include(c => c.Evento).Where(X => X.Estado == true)
                .Where(x => x.CotizacionID == CotizacionID && x.Estado==true)
                .OrderByDescending(x => x.CotizacionID)
                .Select(cotizacion => new GetAllCotizacionModel()
                {
                    CotizacionID = cotizacion.CotizacionID,
                    ClienteID = cotizacion.ClienteID,
                    ClienteNombre = cotizacion.Cliente.NombreCompleto,
                    ClienteDocumento = cotizacion.Cliente.NumeroDocumento,
                    ClienteTelefono = cotizacion.Cliente.Telefono,
                    ClienteEmail = cotizacion.Cliente.Email,
                    LocalID = cotizacion.LocalID,
                    LocalNombre = cotizacion.Local.Nombre,
                    LocalDireccion = cotizacion.Local.Direccion,
                    LocalCapacidad = cotizacion.Local.Capacidad,
                    LocalPrecioAlquiler = cotizacion.Local.PrecioAlquiler,
                    LocalHorasEvento = cotizacion.Local.HorasEvento,
                    EventoID = cotizacion.EventoID,
                    EventoNombre = cotizacion.Evento != null ? cotizacion.Evento.Nombre : null,
                    EventoDescripcion = cotizacion.Evento != null ? cotizacion.Evento.Descripcion : null,
                    FechaTentativa = cotizacion.FechaTentativa,
                    FechaTentativaOpcional = cotizacion.FechaTentativaOpcional,
                    FechaContacto = cotizacion.FechaContacto,
                    HoraContacto = cotizacion.HoraContacto,
                    NumeroInvitados = cotizacion.NumeroInvitados,
                    CostoDePersonal = cotizacion.CostoDePersonal,
                    Garantia = cotizacion.Garantia,
                    TarifaMenuPorInvitado = cotizacion.TarifaMenuPorInvitado,
                    SubtotalMenu = cotizacion.SubtotalMenu,
                    TotalEvento = cotizacion.TotalEvento,
                    PrecioPorCubierto = cotizacion.PrecioPorCubierto,
                    PrecioPorCubiertoConDescuento = cotizacion.PrecioPorCubiertoConDescuento,
                    TotalCotizacion = cotizacion.TotalCotizacion,
                    Observacion = cotizacion.Observacion,
                    EstadoCotizacion = cotizacion.EstadoCotizacion,
                    UsuarioCreacion = cotizacion.UsuarioCreacion,
                    FechaCreacion = cotizacion.FechaCreacion,
                    UsuarioModificacion = cotizacion.UsuarioModificacion,
                    FechaModificacion = cotizacion.FechaModificacion,
                    UsuarioEliminacion = cotizacion.UsuarioEliminacion,
                    FechaEliminacion = cotizacion.FechaEliminacion,
                    Estado = cotizacion.Estado,
                    CotizacionProducto = cotizacion.CotizacionProducto.Select(cotizacionProducto => new GetAllCotizacionProductoModel()
                    {
                        CotizacionID = cotizacionProducto.CotizacionID,
                        ProductoID = cotizacionProducto.ProductoID,
                        Cantidad = cotizacionProducto.Cantidad,
                        UsuarioCreacion = cotizacionProducto.UsuarioCreacion,
                        FechaCreacion = cotizacionProducto.FechaCreacion,
                        UsuarioModificacion = cotizacionProducto.UsuarioModificacion,
                        FechaModificacion = cotizacionProducto.FechaModificacion,
                        UsuarioEliminacion = cotizacionProducto.UsuarioEliminacion,
                        FechaEliminacion = cotizacionProducto.FechaEliminacion,
                        Estado = cotizacionProducto.Estado,
                        Producto = new GetAllProductoModel()
                        {
                            ProductoID = cotizacionProducto.Producto.ProductoID,
                            Nombre = cotizacionProducto.Producto.Nombre,
                            Descripcion = cotizacionProducto.Producto.Descripcion,
                            Precio = cotizacionProducto.Producto.Precio,
                            PrecioCosto = cotizacionProducto.Producto.PrecioCosto,
                            FotosUrls = string.IsNullOrEmpty(cotizacionProducto.Producto.Fotos)
    ? new List<string>()
    : cotizacionProducto.Producto.Fotos.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList(),
                            CategoriaID = cotizacionProducto.Producto.CategoriaID,
                            Categoria = cotizacionProducto.Producto.Categoria.Nombre,
                            UsuarioCreacion = cotizacionProducto.Producto.UsuarioCreacion,
                            FechaCreacion = cotizacionProducto.Producto.FechaCreacion,
                            UsuarioModificacion = cotizacionProducto.Producto.UsuarioModificacion,
                            FechaModificacion = cotizacionProducto.Producto.FechaModificacion,
                            UsuarioEliminacion = cotizacionProducto.Producto.UsuarioEliminacion,
                            FechaEliminacion = cotizacionProducto.Producto.FechaEliminacion,
                            Estado = cotizacionProducto.Producto.Estado
                        }
                    }).ToList(),
                    CotizacionServicio = cotizacion.CotizacionServicio.Select(cotizacionServicio => new GetAllCotizacionServicioModel()
                    {
                        CotizacionID = cotizacionServicio.CotizacionID,
                        ServicioID = cotizacionServicio.ServicioID,
                        Cantidad = cotizacionServicio.Cantidad,
                        UsuarioCreacion = cotizacionServicio.UsuarioCreacion,
                        FechaCreacion = cotizacionServicio.FechaCreacion,
                        UsuarioModificacion = cotizacionServicio.UsuarioModificacion,
                        FechaModificacion = cotizacionServicio.FechaModificacion,
                        UsuarioEliminacion = cotizacionServicio.UsuarioEliminacion,
                        FechaEliminacion = cotizacionServicio.FechaEliminacion,
                        Estado = cotizacionServicio.Estado,
                        ServicioAdicional = new GetAllServicioAdicionalModel()
                        {
                            ServicioID = cotizacionServicio.Servicio.ServicioID,
                            Nombre = cotizacionServicio.Servicio.Nombre,
                            Descripcion = cotizacionServicio.Servicio.Descripcion,
                            Precio = cotizacionServicio.Servicio.Precio,
                            UsuarioCreacion = cotizacionServicio.Servicio.UsuarioCreacion,
                            FechaCreacion = cotizacionServicio.Servicio.FechaCreacion,
                            UsuarioModificacion = cotizacionServicio.Servicio.UsuarioModificacion,
                            FechaModificacion = cotizacionServicio.Servicio.FechaModificacion,
                            UsuarioEliminacion = cotizacionServicio.Servicio.UsuarioEliminacion,
                            FechaEliminacion = cotizacionServicio.Servicio.FechaEliminacion,
                            Estado = cotizacionServicio.Servicio.Estado
                        }
                    }).ToList(),
                }).ToListAsync();

            return result;
        }
    }
}
