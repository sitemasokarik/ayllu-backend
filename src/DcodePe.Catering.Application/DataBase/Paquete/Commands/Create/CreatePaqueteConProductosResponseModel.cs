namespace DcodePe.Catering.Application.DataBase.Paquete.Commands.Create
{
    public class CreatePaqueteConProductosResponseModel
    {
        public int PaqueteID { get; set; }
        
        public string Nombre { get; set; }
        
        public string Descripcion { get; set; }
        
        public decimal PrecioTotal { get; set; }
        
        public int TotalProductos { get; set; }
        
        public int TotalServicios { get; set; }
        
        public DateTime? FechaCreacion { get; set; }
    }
}
