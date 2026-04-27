using DcodePe.Catering.Application.DataBase.ServicioAdicional.Queries.GetAllServicioAdicional;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetAllCotizacion
{
    public class GetAllCotizacionServicioModel
    {
        public int CotizacionID { get; set; }

        public int ServicioID { get; set; }

        public int Cantidad { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsuarioEliminacion { get; set; }

        public DateTime? FechaEliminacion { get; set; }

        public bool? Estado { get; set; }

        public virtual GetAllServicioAdicionalModel ServicioAdicional { get; set; }
    }
}
