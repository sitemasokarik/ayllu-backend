namespace DcodePe.Catering.Application.DataBase.PaqueteServicio.Queries.GetAllPaqueteServicio
{
    public class GetAllPaqueteServicioQuery(IDataBaseService databaseService) : IGetAllPaqueteServicioQuery
    {
        private readonly IDataBaseService _databaseService = databaseService;
        public async Task<List<GetAllPaqueteServicioModel>> GetAllPaqueteServicio()
        {
            var result = await _databaseService.PaqueteServicio.Where(x => x.Estado == true)
                .Include(x => x.Servicio).Where(x => x.Servicio.Estado == true)
                .Include(x => x.Paquete).Where(x => x.Paquete.Estado == true)
                .Select(paqueteServicio => new GetAllPaqueteServicioModel()
                {
                    PaqueteID = paqueteServicio.PaqueteID,
                    ServicioID = paqueteServicio.ServicioID,
                    UsuarioCreacion = paqueteServicio.UsuarioCreacion,
                    FechaCreacion = paqueteServicio.FechaCreacion,
                    UsuarioModificacion = paqueteServicio.UsuarioModificacion,
                    FechaModificacion = paqueteServicio.FechaModificacion,
                    UsuarioEliminacion = paqueteServicio.UsuarioEliminacion,
                    FechaEliminacion = paqueteServicio.FechaEliminacion,
                    Estado = paqueteServicio.Estado,
                    Servicio = paqueteServicio.Servicio == null ? null : new GetAllServicioAdicionalModel()
                    {
                        ServicioID = paqueteServicio.Servicio!.ServicioID,
                        Nombre = paqueteServicio.Servicio.Nombre,
                        Descripcion = paqueteServicio.Servicio.Descripcion,
                        Precio = paqueteServicio.Servicio.Precio,
                        UsuarioCreacion = paqueteServicio.Servicio.UsuarioCreacion,
                        FechaCreacion = paqueteServicio.Servicio.FechaCreacion,
                        UsuarioModificacion = paqueteServicio.Servicio.UsuarioModificacion,
                        FechaModificacion = paqueteServicio.Servicio.FechaModificacion,
                        UsuarioEliminacion = paqueteServicio.Servicio.UsuarioEliminacion,
                        FechaEliminacion = paqueteServicio.Servicio.FechaEliminacion,
                        Estado = paqueteServicio.Servicio.Estado
                    },
                }).ToListAsync();


            return result;
        }
    }
}
