namespace DcodePe.Catering.Application.DataBase.Categoria.Commands.Create
{
    public class CreateCategoriaModel
    {
        /// <summary>
        /// ID de la categoría padre (null si es raíz)
        /// </summary>
        public int? CategoriaPadreID { get; set; }
        
        public string Nombre { get; set; }
        
        public string Descripcion { get; set; }
        
        /// <summary>
        /// Nivel de profundidad (se calcula automáticamente si tiene padre)
        /// </summary>
        public int Nivel { get; set; }
        
        /// <summary>
        /// Orden de visualización dentro del mismo nivel
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
        
        public string UsuarioCreacion { get; set; }
    }
}
