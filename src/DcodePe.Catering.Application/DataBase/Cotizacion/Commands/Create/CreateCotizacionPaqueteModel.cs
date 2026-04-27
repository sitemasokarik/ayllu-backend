namespace DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Create
{
    public class CreateCotizacionPaqueteModel
    {
        public int CotizacionID { get; set; }

        public int PaqueteID { get; set; }

        public int Cantidad { get; set; }

        public string UsuarioCreacion { get; set; }

        public bool? Estado { get; set; }
    }
}
