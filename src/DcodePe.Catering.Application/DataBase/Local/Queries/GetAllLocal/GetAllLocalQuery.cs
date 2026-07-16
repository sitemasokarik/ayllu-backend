using System.Linq;

namespace DcodePe.Catering.Application.DataBase.Local.Queries.GetAllLocal
{
    public class GetAllLocalQuery(IDataBaseService dataBaseService) : IGetAllLocalQuery
    {
        private readonly IDataBaseService _dataBaseService = dataBaseService;
        
        public async Task<List<GetAllLocalModel>> GetAllLocals()
        {
            var result = await _dataBaseService.Local
                .Where(local => local.Estado == true)
                .Select(local => new GetAllLocalModel
                {
                    LocalID = local.LocalID,
                    Nombre = local.Nombre,
                    Direccion = local.Direccion,
                    Capacidad = local.Capacidad,
                    PrecioAlquiler = local.PrecioAlquiler,
                    Garantia = local.Garantia,
                    HorasEvento = local.HorasEvento,
                    //Fotos = local.Fotos,
                    FotosUrls = string.IsNullOrEmpty(local.Fotos)
                        ? new List<string>()
                        : local.Fotos.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    TerminosCondiciones = local.TerminosCondiciones,
                    UsuarioCreacion = local.UsuarioCreacion,
                    FechaCreacion = local.FechaCreacion,
                    UsuarioModificacion = local.UsuarioModificacion,
                    FechaModificacion = local.FechaModificacion,
                    UsuarioEliminacion = local.UsuarioEliminacion,
                    FechaEliminacion = local.FechaEliminacion,
                    Estado = local.Estado
                }).ToListAsync();
                
            return result;
        }

        public async Task<GetAllLocalModel> GetAllLocalsById(int idLocal)
        {
            var result = await _dataBaseService.Local
                .Where(local => local.LocalID == idLocal)
                .Select(local => new GetAllLocalModel
                {
                    LocalID = local.LocalID,
                    Nombre = local.Nombre,
                    Direccion = local.Direccion,
                    Capacidad = local.Capacidad,
                    PrecioAlquiler = local.PrecioAlquiler,
                    Garantia = local.Garantia,
                    HorasEvento = local.HorasEvento,
                    //Fotos = local.Fotos,
                    FotosUrls = string.IsNullOrEmpty(local.Fotos)
                        ? new List<string>()
                        : local.Fotos.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    TerminosCondiciones = local.TerminosCondiciones,
                    UsuarioCreacion = local.UsuarioCreacion,
                    FechaCreacion = local.FechaCreacion,
                    UsuarioModificacion = local.UsuarioModificacion,
                    FechaModificacion = local.FechaModificacion,
                    UsuarioEliminacion = local.UsuarioEliminacion,
                    FechaEliminacion = local.FechaEliminacion,
                    Estado = local.Estado
                }).FirstOrDefaultAsync();

            return result;
        }
    }
}
