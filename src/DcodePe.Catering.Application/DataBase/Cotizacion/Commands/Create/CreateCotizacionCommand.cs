using DcodePe.Catering.Domain.Entities;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Create
{
    public class CreateCotizacionCommand(IDataBaseService databaseService, IMapper mapper) : ICreateCotizacionCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;
        private readonly IMapper _mapper = mapper;

        public async Task<CreateCotizacionModel> ExecuteSaveCotizacion(CreateCotizacionModel model)
        {
            var entity = _mapper.Map<CotizacionEntity>(model);
            entity.FechaCreacion = DateTime.Now;
            entity.Estado = true;
            entity.OrigenCotizacion = NormalizeOrigen(model.OrigenCotizacion);
            if (entity.OrigenCotizacion == "Admin")
            {
                if (model.CreadoPorUsuarioID.HasValue && model.CreadoPorUsuarioID.Value > 0)
                    entity.CreadoPorUsuarioID = model.CreadoPorUsuarioID;

                if (!string.IsNullOrWhiteSpace(model.CreadoPorNombre))
                {
                    entity.CreadoPorNombre = model.CreadoPorNombre.Trim();
                    entity.UsuarioCreacion = entity.CreadoPorNombre;
                }
            }
            else if (entity.OrigenCotizacion == "Landing")
            {
                entity.UsuarioCreacion = string.IsNullOrWhiteSpace(model.UsuarioCreacion)
                    ? "Landing"
                    : model.UsuarioCreacion.Trim();
            }

            entity.CotizacionProducto = new List<CotizacionProductoEntity>();
            entity.CotizacionServicio = new List<CotizacionServicioEntity>();

            await _databaseService.Cotizacion.AddAsync(entity);
            await _databaseService.SaveAsync();
            model.CotizacionID = entity.CotizacionID;

            if (model.CotizacionProducto != null && model.CotizacionProducto.Any())
            {
                var productosDistintos = model.CotizacionProducto
                    .Where(p => p.ProductoID > 0)
                    .GroupBy(p => p.ProductoID)
                    .Select(g => g.First())
                    .ToList();

                if (productosDistintos.Count > 0)
                {
                    var nuevosProductos = productosDistintos.Select(productoModel => new CotizacionProductoEntity
                    {
                        CotizacionID = entity.CotizacionID,
                        ProductoID = productoModel.ProductoID,
                        Cantidad = productoModel.Cantidad > 0 ? productoModel.Cantidad : 1,
                        UsuarioCreacion = model.UsuarioCreacion ?? "SYSTEM",
                        FechaCreacion = DateTime.Now,
                        Estado = true
                    }).ToList();

                    await _databaseService.CotizacionProducto.AddRangeAsync(nuevosProductos);
                }
            }

            if (model.CotizacionServicio != null && model.CotizacionServicio.Any())
            {
                var serviciosDistintos = model.CotizacionServicio
                    .Where(s => s.ServicioID > 0)
                    .GroupBy(s => s.ServicioID)
                    .Select(g => g.First())
                    .ToList();

                if (serviciosDistintos.Count > 0)
                {
                    var nuevosServicios = serviciosDistintos.Select(servicioModel => new CotizacionServicioEntity
                    {
                        CotizacionID = entity.CotizacionID,
                        ServicioID = servicioModel.ServicioID,
                        Cantidad = servicioModel.Cantidad > 0 ? servicioModel.Cantidad : 1,
                        UsuarioCreacion = model.UsuarioCreacion ?? "SYSTEM",
                        FechaCreacion = DateTime.Now,
                        Estado = true
                    }).ToList();

                    await _databaseService.CotizacionServicio.AddRangeAsync(nuevosServicios);
                }
            }

            await _databaseService.SaveAsync();

            return model;
        }

        private static string NormalizeOrigen(string? origen)
        {
            if (string.IsNullOrWhiteSpace(origen))
                return "Admin";

            var value = origen.Trim();
            if (value.Equals("Landing", StringComparison.OrdinalIgnoreCase))
                return "Landing";
            if (value.Equals("Portal", StringComparison.OrdinalIgnoreCase))
                return "Portal";
            return "Admin";
        }
    }
}
