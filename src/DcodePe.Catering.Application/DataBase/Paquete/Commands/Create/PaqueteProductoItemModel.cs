namespace DcodePe.Catering.Application.DataBase.Paquete.Commands.Create
{
    /// <summary>
    /// Representa un producto que será agregado al paquete
    /// </summary>
    public class PaqueteProductoItemModel
    {
        public int ProductoID { get; set; }
        
        public int Cantidad { get; set; }
    }
}
