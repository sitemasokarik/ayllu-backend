namespace DcodePe.Catering.Application.DataBase.Permiso.Queries.GetByRolId
{
    public class GetPermisosByRolIdQuery : IGetPermisosByRolIdQuery
    {
        private readonly IDataBaseService _databaseService;

        public GetPermisosByRolIdQuery(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<GetPermisosByRolIdModel>> Execute(int rolId)
        {
            var permisos = await _databaseService.Permiso
                .Where(p => p.RolID == rolId && p.Estado == true)
                .Include(p => p.Pagina)
                .Select(p => new GetPermisosByRolIdModel
                {
                    PermisoID = p.PermisoID,
                    RolID = p.RolID,
                    PaginaID = p.PaginaID,
                    PaginaNombre = p.Pagina.Nombre,
                    PaginaDescripcion = p.Pagina.Descripcion,
                    PuedeVer = p.PuedeVer ?? false,
                    PuedeCrear = p.PuedeCrear ?? false,
                    PuedeEditar = p.PuedeEditar ?? false,
                    PuedeEliminar = p.PuedeEliminar ?? false,
                    FechaCreacion = p.FechaCreacion,
                    Estado = p.Estado
                })
                .ToListAsync();

            return permisos;
        }
    }
}
