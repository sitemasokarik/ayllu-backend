using DcodePe.Catering.Domain.Entities;

namespace DcodePe.Catering.Application.DataBase.Paquete.Commands.Create
{
    public class CreatePaqueteConProductosCommand : ICreatePaqueteConProductosCommand
    {
        private readonly IDataBaseService _databaseService;

        public CreatePaqueteConProductosCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<CreatePaqueteConProductosResponseModel> ExecuteSavePaqueteConProductos(CreatePaqueteProductosModel model)
        {
            try
            {
                // 1. Crear el paquete principal
                var paquete = new PaqueteEntity
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    PrecioTotal = model.PrecioTotal,
                    UsuarioCreacion = model.UsuarioCreacion,
                    FechaCreacion = DateTime.Now,
                    Estado = true
                };

                await _databaseService.Paquete.AddAsync(paquete);
                await _databaseService.SaveAsync();

                // 2. Agregar productos al paquete con cantidad
                if (model.Productos != null && model.Productos.Any())
                {
                    foreach (var productoItem in model.Productos)
                    {
                        var paqueteProducto = new PaqueteProductoEntity
                        {
                            PaqueteID = paquete.PaqueteID,
                            ProductoID = productoItem.ProductoID,
                            Cantidad = productoItem.Cantidad,
                            UsuarioCreacion = model.UsuarioCreacion,
                            FechaCreacion = DateTime.Now,
                            Estado = true
                        };

                        await _databaseService.PaqueteProducto.AddAsync(paqueteProducto);
                    }
                    
                    await _databaseService.SaveAsync();
                }

                // 3. Agregar servicios al paquete con cantidad
                if (model.Servicios != null && model.Servicios.Any())
                {
                    foreach (var servicioItem in model.Servicios)
                    {
                        var paqueteServicio = new PaqueteServicioEntity
                        {
                            PaqueteID = paquete.PaqueteID,
                            ServicioID = servicioItem.ServicioID,
                            Cantidad = servicioItem.Cantidad,
                            UsuarioCreacion = model.UsuarioCreacion,
                            FechaCreacion = DateTime.Now,
                            Estado = true
                        };

                        await _databaseService.PaqueteServicio.AddAsync(paqueteServicio);
                    }
                    
                    await _databaseService.SaveAsync();
                }

                // 4. Retornar respuesta
                return new CreatePaqueteConProductosResponseModel
                {
                    PaqueteID = paquete.PaqueteID,
                    Nombre = paquete.Nombre,
                    Descripcion = paquete.Descripcion,
                    PrecioTotal = paquete.PrecioTotal,
                    TotalProductos = model.Productos?.Count ?? 0,
                    TotalServicios = model.Servicios?.Count ?? 0,
                    FechaCreacion = paquete.FechaCreacion
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
