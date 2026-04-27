namespace DcodePe.Catering.Application.DataBase.Categoria.Queries.GetById
{
    public class GetCategoriaByIdModel
    {
        public int CategoriaID { get; set; }
        
        public string Nombre { get; set; }
        
        public string Descripcion { get; set; }
        
        public int Limite { get; set; }
        
        /// <summary>
        /// ID de la categoría padre (null si es categoría raíz)
        /// </summary>
        public int? CategoriaPadreID { get; set; }
        
        /// <summary>
        /// Nombre de la categoría padre (null si es categoría raíz)
        /// </summary>
        public string CategoriaPadreNombre { get; set; }
        
        public int TotalProductos { get; set; }
        
        public string UsuarioCreacion { get; set; }
        
        public DateTime? FechaCreacion { get; set; }
        
        public string UsuarioModificacion { get; set; }
        
        public DateTime? FechaModificacion { get; set; }
        
        public bool? Estado { get; set; }
    }
}
