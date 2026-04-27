namespace DcodePe.Catering.Application.DataBase.Categoria.Queries.GetAll
{
    public interface IGetAllCategoriaQuery
    {
        /// <summary>
        /// Obtiene lista plana de todas las categorías (sin jerarquía)
        /// </summary>
        Task<List<GetAllCategoriaModel>> Execute();
        
        /// <summary>
        /// Obtiene toda la jerarquía de categorías desde la raíz (recursivo)
        /// </summary>
        Task<List<GetAllCategoriaModel>> ExecuteHierarchy();
        
        /// <summary>
        /// Obtiene una categoría con todos sus hijos (recursivo)
        /// </summary>
        Task<GetAllCategoriaModel?> ExecuteGetByIdWithChildren(int categoriaId);
        
        /// <summary>
        /// Obtiene solo categorías raíz con primer nivel de hijos
        /// </summary>
        Task<List<GetAllCategoriaModel>> ExecuteGetRootCategories();
        
        /// <summary>
        /// Obtiene el path completo de una categoría (breadcrumb recursivo)
        /// </summary>
        Task<List<GetAllCategoriaModel>> ExecuteGetCategoryPath(int categoriaId);
        
        /// <summary>
        /// Obtiene todos los descendientes de una categoría (recursivo)
        /// </summary>
        Task<List<int>> ExecuteGetAllDescendants(int categoriaId);
        
        /// <summary>
        /// Valida que no haya círculos en la jerarquía (recursivo)
        /// </summary>
        Task<bool> ValidateHierarchyWithoutCircles(int categoriaId, int? nuevoPadreId);
    }
}
