namespace DcodePe.Catering.Application.DataBase.Pagina.Queries.GetById
{
    public class GetPaginaByIdQuery : IGetPaginaByIdQuery
    {
        private readonly IDataBaseService _databaseService;

        public GetPaginaByIdQuery(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<GetPaginaByIdModel> Execute(int paginaId)
        {
            var pagina = await _databaseService.Pagina
                .Where(p => p.PaginaID == paginaId && p.Estado == true)
                .Select(p => new GetPaginaByIdModel
                {
                    PaginaID = p.PaginaID,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Url = p.Url,
                    Icono = p.Icono,
                    GrupoMenu = p.GrupoMenu,
                    OrdenMenu = p.OrdenMenu,
                    FechaCreacion = p.FechaCreacion,
                    UsuarioCreacion = p.UsuarioCreacion,
                    Estado = p.Estado
                })
                .FirstOrDefaultAsync();

            return pagina;
        }
    }
}
