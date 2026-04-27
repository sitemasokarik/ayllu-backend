using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Categoria.Queries.GetById
{
    public class GetCategoriaByIdQuery : IGetCategoriaByIdQuery
    {
        private readonly IDataBaseService _databaseService;

        public GetCategoriaByIdQuery(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<GetCategoriaByIdModel> Execute(int categoriaId)
        {
            var categoria = await _databaseService.Categoria
                .Include(c => c.CategoriaPadre).Where(x => x.Estado == true) // Incluir categoría padre
                .Where(c => c.CategoriaID == categoriaId && c.Estado == true)
                .Select(c => new GetCategoriaByIdModel
                {
                    CategoriaID = c.CategoriaID,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    Limite = c.Limite,
                    CategoriaPadreID = c.CategoriaPadreID, // NUEVO
                    CategoriaPadreNombre = c.CategoriaPadre != null ? c.CategoriaPadre.Nombre : null, // NUEVO
                    TotalProductos = c.Productos.Count(p => p.Estado == true),
                    UsuarioCreacion = c.UsuarioCreacion,
                    FechaCreacion = c.FechaCreacion,
                    UsuarioModificacion = c.UsuarioModificacion,
                    FechaModificacion = c.FechaModificacion,
                    Estado = c.Estado
                })
                .FirstOrDefaultAsync();

            return categoria;
        }
    }
}
