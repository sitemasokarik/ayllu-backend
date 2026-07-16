using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Pagina.Queries.GetAll
{
    public class GetAllPaginaQuery : IGetAllPaginaQuery
    {
        private readonly IDataBaseService _databaseService;

        public GetAllPaginaQuery(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<GetAllPaginaModel>> Execute()
        {
            var paginas = await _databaseService.Pagina
                .Where(p => p.Estado == true)
                .OrderBy(p => p.GrupoMenu)
                .ThenBy(p => p.OrdenMenu ?? 999)
                .ThenBy(p => p.Nombre)
                .Select(p => new GetAllPaginaModel
                {
                    PaginaID = p.PaginaID,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Url = p.Url,
                    Icono = p.Icono,
                    GrupoMenu = p.GrupoMenu,
                    OrdenMenu = p.OrdenMenu,
                    TotalPermisos = p.Permiso.Count(per => per.Estado == true),
                    FechaCreacion = p.FechaCreacion,
                    UsuarioCreacion = p.UsuarioCreacion,
                    Estado = p.Estado
                })
                .ToListAsync();

            return paginas;
        }
    }
}
