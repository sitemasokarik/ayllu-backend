using DcodePe.Catering.Application.DataBase.Usuario.Queries.GetAllUsuario;

using System.Collections.Generic;

namespace DcodePe.Catering.Application.DataBase.Evento.Queries.GetAllEvento
{
    public class GetAllEventoModel
    {
        public int EventoID { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public List<string> FotosUrls { get; set; }

        public string EstadoEvento { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsuarioEliminacion { get; set; }

        public DateTime? FechaEliminacion { get; set; }

        public bool? Estado { get; set; }

        public string TarifasInvitadoJson { get; set; }
    }
}
