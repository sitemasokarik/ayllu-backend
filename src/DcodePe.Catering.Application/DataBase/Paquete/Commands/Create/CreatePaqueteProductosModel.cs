namespace DcodePe.Catering.Application.DataBase.Paquete.Commands.Create
{
    public class CreatePaqueteProductosModel
    {
        public string Nombre { get; set; }
        
        public string Descripcion { get; set; }
        
        public decimal PrecioTotal { get; set; }
        
        public string UsuarioCreacion { get; set; }
        
        /// <summary>
        /// Lista de productos que formarán parte del paquete
        /// </summary>
        public List<PaqueteProductoItemModel> Productos { get; set; } = new();
        
        /// <summary>
        /// Lista de servicios que formarán parte del paquete
        /// </summary>
        public List<PaqueteServicioItemModel> Servicios { get; set; } = new();
    }
}
