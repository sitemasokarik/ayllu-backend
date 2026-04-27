namespace DcodePe.Catering.Application.DataBase.ServicioAdicional.Queries.GetAllServicioAdicional
{
    public class GetServicioAdicionalQuery : IGetServicioAdicionalQuery
    {
        private readonly IDataBaseService _databaseService;

        public GetServicioAdicionalQuery(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<GetAllServicioAdicionalModel>> GetAllServicioAdicionalAsync()
        {
            var servicios = await _databaseService.ServicioAdicional
                .Where(s => s.Estado == true)
                .ToListAsync();

            return servicios.Select(servicio => new GetAllServicioAdicionalModel
            {
                ServicioID = servicio.ServicioID,
                Nombre = servicio.Nombre,
                Descripcion = servicio.Descripcion,
                Precio = servicio.Precio,
                FotosUrls = servicio.FotosUrls?.ToList() ?? new List<string>(),
                UsuarioCreacion = servicio.UsuarioCreacion,
                FechaCreacion = servicio.FechaCreacion,
                UsuarioModificacion = servicio.UsuarioModificacion,
                FechaModificacion = servicio.FechaModificacion,
                UsuarioEliminacion = servicio.UsuarioEliminacion,
                FechaEliminacion = servicio.FechaEliminacion,
                Estado = servicio.Estado
            }).ToList();
        }

        public async Task<GetAllServicioAdicionalModel> GetServicioAdicionalByIdAsync(int servicioId)
        {
            var servicio = await _databaseService.ServicioAdicional
                .Where(s => s.ServicioID == servicioId)
                .FirstOrDefaultAsync();

            if (servicio == null)
                return null;

            return new GetAllServicioAdicionalModel
            {
                ServicioID = servicio.ServicioID,
                Nombre = servicio.Nombre,
                Descripcion = servicio.Descripcion,
                Precio = servicio.Precio,
                FotosUrls = servicio.FotosUrls?.ToList() ?? new List<string>(),
                UsuarioCreacion = servicio.UsuarioCreacion,
                FechaCreacion = servicio.FechaCreacion,
                UsuarioModificacion = servicio.UsuarioModificacion,
                FechaModificacion = servicio.FechaModificacion,
                UsuarioEliminacion = servicio.UsuarioEliminacion,
                FechaEliminacion = servicio.FechaEliminacion,
                Estado = servicio.Estado
            };
        }
    }
}
