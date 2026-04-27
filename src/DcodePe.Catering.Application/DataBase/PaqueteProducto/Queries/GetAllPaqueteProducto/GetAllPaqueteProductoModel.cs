using DcodePe.Catering.Application.DataBase.Paquete.Queries.GetAllPaquete;

namespace DcodePe.Catering.Application.DataBase.PaqueteProducto.Queries.GetAllPaqueteProducto
{
    public class GetAllPaqueteProductoModel
    {
        public int PaqueteID { get; set; }
        public int ProductoID { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioEliminacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public bool? Estado { get; set; }

        //public virtual GetAllPaqueteModel Paquete { get; set; }

        public virtual GetAllProductoModel Producto { get; set; }
    }
}
