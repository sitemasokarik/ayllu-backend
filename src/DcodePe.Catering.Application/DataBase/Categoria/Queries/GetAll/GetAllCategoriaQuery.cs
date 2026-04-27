using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Categoria.Queries.GetAll
{
    public class GetAllCategoriaQuery : IGetAllCategoriaQuery
    {
        private readonly IDataBaseService _databaseService;

        public GetAllCategoriaQuery(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        /// Obtiene lista plana de todas las categorías (sin jerarquía)
        /// </summary>
        public async Task<List<GetAllCategoriaModel>> Execute()
        {
            var categorias = await _databaseService.Categoria
                .Where(c => c.Estado == true)
                .OrderBy(c => c.Nivel)
                .ThenBy(c => c.Orden)
                .Select(c => new GetAllCategoriaModel
                {
                    CategoriaID = c.CategoriaID,
                    CategoriaPadreID = c.CategoriaPadreID,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    Nivel = c.Nivel,
                    Orden = c.Orden,
                    Icono = c.Icono,
                    Limite = c.Limite,
                    TotalProductos = c.Productos.Count(p => p.Estado == true),
                    TotalSubcategorias = c.Subcategorias.Count(s => s.Estado == true),
                    EsHoja = !c.Subcategorias.Any(s => s.Estado == true),
                    FechaCreacion = c.FechaCreacion,
                    UsuarioCreacion = c.UsuarioCreacion,
                    Estado = c.Estado
                })
                .ToListAsync();

            return categorias;
        }

        /// <summary>
        /// Obtiene toda la jerarquía de categorías desde la raíz (recursivo)
        /// </summary>
        public async Task<List<GetAllCategoriaModel>> ExecuteHierarchy()
        {
            // Obtener todas las categorías activas
            var todasLasCategorias = await _databaseService.Categoria
                .Where(c => c.Estado == true)
                .Include(c => c.Productos)
                .OrderBy(c => c.Nivel)
                .ThenBy(c => c.Orden)
                .ToListAsync();

            // Construir jerarquía recursivamente desde la raíz
            var categoriasRaiz = todasLasCategorias
                .Where(c => c.CategoriaPadreID == null)
                .Select(c => MapearCategoriaRecursivamente(c, todasLasCategorias))
                .ToList();

            return categoriasRaiz;
        }

        /// <summary>
        /// Obtiene una categoría específica con toda su jerarquía de hijos (recursivo)
        /// </summary>
        public async Task<GetAllCategoriaModel?> ExecuteGetByIdWithChildren(int categoriaId)
        {
            var todasLasCategorias = await _databaseService.Categoria
                .Where(c => c.Estado == true)
                .Include(c => c.Productos)
                .ToListAsync();

            var categoriaRaiz = todasLasCategorias.FirstOrDefault(c => c.CategoriaID == categoriaId);

            if (categoriaRaiz == null)
                return null;

            return MapearCategoriaRecursivamente(categoriaRaiz, todasLasCategorias);
        }

        /// <summary>
        /// Obtiene solo las categorías raíz (nivel 0) con sus hijos directos
        /// </summary>
        public async Task<List<GetAllCategoriaModel>> ExecuteGetRootCategories()
        {
            var todasLasCategorias = await _databaseService.Categoria
                .Where(c => c.Estado == true)
                .Include(c => c.Productos)
                .OrderBy(c => c.Orden)
                .ToListAsync();

            var categoriasRaiz = todasLasCategorias
                .Where(c => c.CategoriaPadreID == null)
                .Select(c => MapearCategoriaRecursivamente(c, todasLasCategorias, maxNivel: 1))
                .ToList();

            return categoriasRaiz;
        }

        /// <summary>
        /// Obtiene el path completo de una categoría (breadcrumb) - RECURSIVO
        /// </summary>
        public async Task<List<GetAllCategoriaModel>> ExecuteGetCategoryPath(int categoriaId)
        {
            var path = new List<GetAllCategoriaModel>();

            var categoria = await _databaseService.Categoria
                .Where(c => c.CategoriaID == categoriaId && c.Estado == true)
                .FirstOrDefaultAsync();

            if (categoria == null)
                return path;

            // Construir path recursivamente hacia arriba
            await ConstruirPathRecursivo(categoria, path);

            return path;
        }

        /// <summary>
        /// Obtiene todos los descendientes de una categoría (recursivo)
        /// </summary>
        public async Task<List<int>> ExecuteGetAllDescendants(int categoriaId)
        {
            var descendientes = new List<int>();
            await ObtenerDescendientesRecursivo(categoriaId, descendientes);
            return descendientes;
        }

        /// <summary>
        /// Valida que no existan referencias circulares (recursivo)
        /// </summary>
        public async Task<bool> ValidateHierarchyWithoutCircles(int categoriaId, int? nuevoPadreId)
        {
            if (!nuevoPadreId.HasValue)
                return true;

            // No puede ser su propio padre
            if (categoriaId == nuevoPadreId.Value)
                return false;

            // Verificar recursivamente que el nuevo padre no sea descendiente
            return await VerificarNoEsDescendiente(categoriaId, nuevoPadreId.Value);
        }

        // ===== MÉTODOS RECURSIVOS PRIVADOS =====

        /// <summary>
        /// Mapea una categoría con todas sus subcategorías recursivamente
        /// </summary>
        private GetAllCategoriaModel MapearCategoriaRecursivamente(
            Domain.Entities.CategoriaEntity categoria,
            List<Domain.Entities.CategoriaEntity> todasLasCategorias,
            int maxNivel = int.MaxValue,
            int nivelActual = 0,
            string pathPadre = "")
        {
            var model = new GetAllCategoriaModel
            {
                CategoriaID = categoria.CategoriaID,
                CategoriaPadreID = categoria.CategoriaPadreID,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Nivel = categoria.Nivel,
                Orden = categoria.Orden,
                Icono = categoria.Icono,
                Limite = categoria.Limite,
                TotalProductos = categoria.Productos.Count(p => p.Estado == true),
                UsuarioCreacion = categoria.UsuarioCreacion,
                FechaCreacion = categoria.FechaCreacion,
                Estado = categoria.Estado,
                PathCompleto = string.IsNullOrEmpty(pathPadre)
                    ? categoria.Nombre
                    : $"{pathPadre} > {categoria.Nombre}"
            };

            // RECURSIÓN: Mapear subcategorías si no alcanzamos el máximo nivel
            if (nivelActual < maxNivel)
            {
                var subcategorias = todasLasCategorias
                    .Where(c => c.CategoriaPadreID == categoria.CategoriaID)
                    .OrderBy(c => c.Orden)
                    .Select(c => MapearCategoriaRecursivamente(
                        c,
                        todasLasCategorias,
                        maxNivel,
                        nivelActual + 1,
                        model.PathCompleto))
                    .ToList();

                model.Subcategorias = subcategorias;
                model.TotalSubcategorias = subcategorias.Count;
                model.EsHoja = subcategorias.Count == 0;
            }

            return model;
        }

        /// <summary>
        /// Construye el path completo de una categoría recursivamente hacia arriba
        /// </summary>
        private async Task ConstruirPathRecursivo(Domain.Entities.CategoriaEntity categoria, List<GetAllCategoriaModel> path)
        {
            if (categoria == null)
                return;

            // Agregar la categoría actual al inicio del path
            path.Insert(0, new GetAllCategoriaModel
            {
                CategoriaID = categoria.CategoriaID,
                CategoriaPadreID = categoria.CategoriaPadreID,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Nivel = categoria.Nivel,
                Orden = categoria.Orden,
                Icono = categoria.Icono,
                Limite = categoria.Limite,
                Estado = categoria.Estado
            });

            // RECURSIÓN: Si tiene padre, continuar hacia arriba
            if (categoria.CategoriaPadreID.HasValue)
            {
                var padre = await _databaseService.Categoria
                    .FirstOrDefaultAsync(c => c.CategoriaID == categoria.CategoriaPadreID.Value);

                if (padre != null)
                {
                    await ConstruirPathRecursivo(padre, path);
                }
            }
        }

        /// <summary>
        /// Verifica recursivamente si una categoría es descendiente de otra
        /// </summary>
        private async Task<bool> VerificarNoEsDescendiente(int categoriaId, int posibleDescendienteId)
        {
            var posibleDescendiente = await _databaseService.Categoria
                .FirstOrDefaultAsync(c => c.CategoriaID == posibleDescendienteId);

            if (posibleDescendiente == null)
                return true;

            // Si el posible descendiente es la categoría original, hay un círculo
            if (posibleDescendiente.CategoriaID == categoriaId)
                return false;

            // RECURSIÓN: Si tiene padre, verificar hacia arriba
            if (posibleDescendiente.CategoriaPadreID.HasValue)
            {
                return await VerificarNoEsDescendiente(categoriaId, posibleDescendiente.CategoriaPadreID.Value);
            }

            return true;
        }

        /// <summary>
        /// Método recursivo para obtener todos los descendientes
        private async Task ObtenerDescendientesRecursivo(int categoriaId, List<int> descendientes)
        {
            var hijos = await _databaseService.Categoria
                .Where(c => c.CategoriaPadreID == categoriaId && c.Estado == true)
                .Select(c => c.CategoriaID)
                .ToListAsync();

            foreach (var hijoId in hijos)
            {
                descendientes.Add(hijoId);
                // RECURSIÓN: Obtener descendientes de cada hijo
                await ObtenerDescendientesRecursivo(hijoId, descendientes);
            }
        }
    }
}
