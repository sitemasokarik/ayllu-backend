namespace DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetAllCotizacion
{
    public class GetAllCotizacionProductoModel
    {
        public int CotizacionID { get; set; }

        public int ProductoID { get; set; }

        public int Cantidad { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsuarioEliminacion { get; set; }

        public DateTime? FechaEliminacion { get; set; }

        public bool? Estado { get; set; }

        public virtual GetAllProductoModel Producto { get; set; }
    }
}
