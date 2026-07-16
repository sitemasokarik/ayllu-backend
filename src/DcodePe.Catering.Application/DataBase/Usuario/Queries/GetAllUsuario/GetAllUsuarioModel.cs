using DcodePe.Catering.Application.DataBase.Evento.Queries.GetAllEvento;

namespace DcodePe.Catering.Application.DataBase.Usuario.Queries.GetAllUsuario
{
    public class GetAllUsuarioModel
    {
        public int UsuarioID { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int RolID { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsuarioEliminacion { get; set; }

        public DateTime? FechaEliminacion { get; set; }

        public bool? Estado { get; set; }

        //public virtual ICollection<GetAllEventoModel> Evento { get; set; } = new List<GetAllEventoModel>();
    }
}
