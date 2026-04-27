namespace DcodePe.Catering.Application.DataBase.Rol.Queries.GetById
{
    public class GetRolByIdQuery : IGetRolByIdQuery
    {
        private readonly IDataBaseService _databaseService;

        public GetRolByIdQuery(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<GetRolByIdModel> Execute(int rolId)
        {
            var rol = await _databaseService.Rol
                .Where(r => r.RolID == rolId && r.Estado == true)
                .Select(r => new GetRolByIdModel
                {
                    RolID = r.RolID,
                    Nombre = r.Nombre,
                    Descripcion = r.Descripcion,
                    FechaCreacion = r.FechaCreacion,
                    UsuarioCreacion = r.UsuarioCreacion,
                    Estado = r.Estado,
                    Permisos = r.Permiso
                        .Where(p => p.Estado == true)
                        .Select(p => new PermisoResumenModel
                        {
                            PermisoID = p.PermisoID,
                            PaginaID = p.PaginaID,
                            PaginaNombre = p.Pagina.Nombre,
                            Url = p.Pagina.Url,
                            Icono = p.Pagina.Icono,
                            PuedeVer = p.PuedeVer ?? false,
                            PuedeCrear = p.PuedeCrear ?? false,
                            PuedeEditar = p.PuedeEditar ?? false,
                            PuedeEliminar = p.PuedeEliminar ?? false
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            return rol;
        }
    }
}
