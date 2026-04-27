using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Permiso.Queries.GetAll
{
    public class GetAllPermisoQuery : IGetAllPermisoQuery
    {
        private readonly IDataBaseService _databaseService;

        public GetAllPermisoQuery(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<GetAllPermisoModel>> Execute()
        {
            var permisos = await _databaseService.Permiso
                .Where(p => p.Estado == true)
                .Include(p => p.Rol).Where(r => r.Estado == true)
                .Include(p => p.Pagina).Where(pg => pg.Estado == true)
                .OrderBy(p => p.Rol.Nombre)
                .ThenBy(p => p.Pagina.Nombre)
                .Select(p => new GetAllPermisoModel
                {
                    PermisoID = p.PermisoID,
                    RolID = p.RolID,
                    RolNombre = p.Rol.Nombre,
                    PaginaID = p.PaginaID,
                    PaginaNombre = p.Pagina.Nombre,
                    PuedeVer = p.PuedeVer ?? false,
                    PuedeCrear = p.PuedeCrear ?? false,
                    PuedeEditar = p.PuedeEditar ?? false,
                    PuedeEliminar = p.PuedeEliminar ?? false,
                    FechaCreacion = p.FechaCreacion,
                    UsuarioCreacion = p.UsuarioCreacion,
                    Estado = p.Estado
                })
                .ToListAsync();

            return permisos;
        }
    }
}
