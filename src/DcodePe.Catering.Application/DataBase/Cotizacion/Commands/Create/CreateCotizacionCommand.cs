using DcodePe.Catering.Domain.Entities;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Create
{
    public class CreateCotizacionCommand(IDataBaseService databaseService, IMapper mapper) : ICreateCotizacionCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;
        private readonly IMapper _mapper = mapper;

        public async Task<CreateCotizacionModel> ExecuteSaveCotizacion(CreateCotizacionModel model)
        {
            // Mapear la entidad principal (sin las colecciones anidadas)
            var entity = _mapper.Map<CotizacionEntity>(model);
            entity.FechaCreacion = DateTime.Now;
            entity.Estado = true;

            // Agregar la cotización a la base de datos
            await _databaseService.Cotizacion.AddAsync(entity);
            await _databaseService.SaveAsync();

            // Ahora manejar las colecciones anidadas si existen
            if (model.CotizacionProducto != null && model.CotizacionProducto.Any())
            {
                foreach (var productoModel in model.CotizacionProducto)
                {
                    var cotizacionProducto = new CotizacionProductoEntity
                    {
                        CotizacionID = entity.CotizacionID,
                        ProductoID = productoModel.ProductoID,
                        Cantidad = productoModel.Cantidad,
                        UsuarioCreacion = model.UsuarioCreacion ?? "SYSTEM",
                        FechaCreacion = DateTime.Now,
                        Estado = true
                    };
                    await _databaseService.CotizacionProducto.AddAsync(cotizacionProducto);
                }
            }

            if (model.CotizacionServicio != null && model.CotizacionServicio.Any())
            {
                foreach (var servicioModel in model.CotizacionServicio)
                {
                    var cotizacionServicio = new CotizacionServicioEntity
                    {
                        CotizacionID = entity.CotizacionID,
                        ServicioID = servicioModel.ServicioID,
                        Cantidad = servicioModel.Cantidad,
                        UsuarioCreacion = model.UsuarioCreacion ?? "SYSTEM",
                        FechaCreacion = DateTime.Now,
                        Estado = true
                    };
                    await _databaseService.CotizacionServicio.AddAsync(cotizacionServicio);
                }
            }

            //if (model.CotizacionPaquete != null && model.CotizacionPaquete.Any())
            //{
            //    foreach (var paqueteModel in model.CotizacionPaquete)
            //    {
            //        var cotizacionPaquete = new CotizacionPaqueteEntity
            //        {
            //            CotizacionID = entity.CotizacionID,
            //            PaqueteID = paqueteModel.PaqueteID,
            //            Cantidad = paqueteModel.Cantidad,
            //            UsuarioCreacion = model.UsuarioCreacion ?? "SYSTEM",
            //            FechaCreacion = DateTime.Now,
            //            Estado = true
            //        };
            //        await _databaseService.CotizacionPaquete.AddAsync(cotizacionPaquete);
            //    }
            //}

            // Guardar todas las relaciones
            await _databaseService.SaveAsync();

            return model;
        }
    }
}
