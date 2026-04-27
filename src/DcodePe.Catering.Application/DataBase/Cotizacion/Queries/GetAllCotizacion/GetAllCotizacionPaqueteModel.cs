namespace DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetAllCotizacion
{
    public class GetAllCotizacionPaqueteModel
    {
        public int CotizacionID { get; set; }

        public int PaqueteID { get; set; }

        public int Cantidad { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsuarioEliminacion { get; set; }

        public DateTime? FechaEliminacion { get; set; }

        public bool? Estado { get; set; }

        public virtual GetCotizacionPaqueteModel Paquete { get; set; }
    }
}
