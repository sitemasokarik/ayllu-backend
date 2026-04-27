using System;
using System.Collections.Generic;

namespace DcodePe.Catering.Application.DataBase.Producto.Queries.GetProducto
{
    public class GetAllProductoModel
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioCosto { get; set; }
        public List<string> FotosUrls { get; set; }
        public int CategoriaID { get; set; }
        public string Categoria { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioEliminacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public bool? Estado { get; set; }
    }
}
