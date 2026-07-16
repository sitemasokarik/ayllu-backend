using DcodePe.Catering.Domain.Entities.Base;

namespace DcodePe.Catering.Domain.Entities.Tickets
{
    public class TicketInternoEntity : BaseEntity
    {
        public int TicketID { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string EstadoTicket { get; set; } = "Abierto";
        public string Prioridad { get; set; } = "Media";
        public int? CreadoPorUsuarioID { get; set; }
        public int? AsignadoUsuarioID { get; set; }
        public int? CreadoPorClienteID { get; set; }
        public int? RolDestinoID { get; set; }
        public int? CotizacionID { get; set; }

        public virtual ICollection<TicketMensajeEntity> Mensajes { get; set; } = new List<TicketMensajeEntity>();
    }
}
