using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Rol.Queries.GetAll
{
    public class GetAllRolQuery : IGetAllRolQuery
    {
        private readonly IDataBaseService _databaseService;

        public GetAllRolQuery(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<GetAllRolModel>> Execute()
        {
            var roles = await _databaseService.Rol
                .Where(r => r.Estado == true)
                .OrderBy(r => r.Nombre)
                .Select(r => new GetAllRolModel
                {
                    RolID = r.RolID,
                    Nombre = r.Nombre,
                    Descripcion = r.Descripcion,
                    TotalUsuarios = r.Usuario.Count(u => u.Estado == true),
                    TotalPermisos = r.Permiso.Count(p => p.Estado == true),
                    FechaCreacion = r.FechaCreacion,
                    UsuarioCreacion = r.UsuarioCreacion,
                    Estado = r.Estado
                })
                .ToListAsync();

            return roles;
        }
    }
}
