namespace DcodePe.Catering.Application.DataBase.Categoria.Commands.Create
{
    public class CreateCategoriaResponseModel
    {
        public int CategoriaID { get; set; }
        
        public string Nombre { get; set; }
        
        public string Descripcion { get; set; }
        
        public int Limite { get; set; }
        
        public DateTime? FechaCreacion { get; set; }
    }
}
