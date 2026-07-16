using System.Collections.Generic;

namespace DcodePe.Catering.Application.DataBase.Evento.Commands.Update
{
    public class UpdateEventoModel
    {
        public int EventoID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<string> FotosUrls { get; set; }
        public string EstadoEvento { get; set; }
        public string UsuarioModificacion { get; set; }
        public string TarifasInvitadoJson { get; set; }
    }
}
