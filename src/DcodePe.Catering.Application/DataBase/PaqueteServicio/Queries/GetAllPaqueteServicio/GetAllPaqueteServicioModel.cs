

namespace DcodePe.Catering.Application.DataBase.PaqueteServicio.Queries.GetAllPaqueteServicio
{
    public class GetAllPaqueteServicioModel
    {
        public int PaqueteID { get; set; }

        public int ServicioID { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsuarioEliminacion { get; set; }

        public DateTime? FechaEliminacion { get; set; }

        public bool? Estado { get; set; }

        //public virtual GetAllPaqueteModel Paquete { get; set; }

        public virtual GetAllServicioAdicionalModel Servicio { get; set; }
    }
}
