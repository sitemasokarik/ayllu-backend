using System.Collections.Generic;

namespace DcodePe.Catering.Application.DataBase.Producto.Commands.Update
{
    public class UpdateProductoModel
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioCosto { get; set; }
        public List<string> FotosUrls { get; set; }
        public int CategoriaID { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
