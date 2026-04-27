namespace DcodePe.Catering.Application.DataBase.PaqueteProducto.Commands.Create
{
    public class CreatePaqueteProductoModel
    {
        public int PaqueteProductoID { get; set; }
        public int PaqueteID { get; set; }
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public DateTime? FechaCreacion { get; set; }

    }
}
