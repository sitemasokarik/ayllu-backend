namespace DcodePe.Catering.Application.DataBase.Paquete.Queries.GetAllPaquete
{
    public class GetPaqueteWithProductosModel
    {
        public int PaqueteID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioTotal { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioEliminacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public bool? Estado { get; set; }
        public virtual List<GetAllProductoModel> Producto { get; set; }
    }
}
