using System.Collections.Generic;

namespace DcodePe.Catering.Application.DataBase.Producto.Commands.Create
{
    public class CreateProductoModel
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioCosto { get; set; }
        public List<string> FotosUrls { get; set; }
        public int CategoriaID { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Estado { get; set; }
    }
}
