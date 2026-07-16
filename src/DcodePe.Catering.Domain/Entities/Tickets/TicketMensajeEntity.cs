using DcodePe.Catering.Domain.Entities.Base;

namespace DcodePe.Catering.Domain.Entities.Tickets
{
    public class TicketMensajeEntity : BaseEntity
    {
        public int TicketMensajeID { get; set; }
        public int TicketID { get; set; }
        public int? UsuarioID { get; set; }
        public int? ClienteID { get; set; }
        public string AutorNombre { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public bool EsInterno { get; set; }

        public virtual TicketInternoEntity Ticket { get; set; } = null!;
    }
}
