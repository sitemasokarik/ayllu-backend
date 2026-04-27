namespace DcodePe.Catering.Application.DataBase.Categoria.Commands.Update
{
    public class UpdateCategoriaModel
    {
        public int CategoriaID { get; set; }
        
        /// <summary>
        /// ID de la categoría padre (null si es raíz)
        /// </summary>
        public int? CategoriaPadreID { get; set; }
        
        public string Nombre { get; set; }
        
        public string Descripcion { get; set; }
        
        /// <summary>
        /// Nivel de profundidad
        /// </summary>
        public int Nivel { get; set; }
        
        /// <summary>
        /// Orden de visualización
        /// </summary>
        public int Orden { get; set; }
        
        /// <summary>
        /// Icono o ruta de imagen
        /// </summary>
        public string? Icono { get; set; }
        
        /// <summary>
        /// Límite de selección para esta categoría
        /// </summary>
        public int Limite { get; set; }
        
        public string UsuarioModificacion { get; set; }
    }
}
