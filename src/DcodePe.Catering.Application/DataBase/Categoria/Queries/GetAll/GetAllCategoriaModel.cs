namespace DcodePe.Catering.Application.DataBase.Categoria.Queries.GetAll
{
    /// <summary>
    /// Modelo que representa una categoría con su jerarquía completa (recursivo)
    /// </summary>
    public class GetAllCategoriaModel
    {
        public int CategoriaID { get; set; }
        
        public int? CategoriaPadreID { get; set; }
        
        public string Nombre { get; set; }
        
        public string Descripcion { get; set; }
        
        /// <summary>
        /// Nivel de profundidad en la jerarquía (0 = raíz)
        /// </summary>
        public int Nivel { get; set; }
        
        /// <summary>
        /// Orden de visualización dentro del mismo nivel
        /// </summary>
        public int Orden { get; set; }
        
        /// <summary>
        /// Icono o imagen de la categoría
        /// </summary>
        public string? Icono { get; set; }
        
        /// <summary>
        /// Límite de selección para esta categoría
        /// </summary>
        public int Limite { get; set; }
        
        /// <summary>
        /// Total de productos asociados directamente a esta categoría
        /// </summary>
        public int TotalProductos { get; set; }
        
        /// <summary>
        /// Total de subcategorías hijas
        /// </summary>
        public int TotalSubcategorias { get; set; }
        
        /// <summary>
        /// Indica si es una categoría hoja (sin subcategorías)
        /// </summary>
        public bool EsHoja { get; set; }
        
        /// <summary>
        /// Path completo de la categoría para breadcrumb (ej: "Alimentos > Entradas > Frías")
        /// </summary>
        public string PathCompleto { get; set; }
        
        public DateTime? FechaCreacion { get; set; }
        
        public string UsuarioCreacion { get; set; }
        
        public bool? Estado { get; set; }
        
        // ===== RECURSIVIDAD: Subcategorías hijas =====
        /// <summary>
        /// Lista de subcategorías hijas (permite navegación recursiva)
        /// </summary>
        public List<GetAllCategoriaModel> Subcategorias { get; set; } = new List<GetAllCategoriaModel>();
    }
}
