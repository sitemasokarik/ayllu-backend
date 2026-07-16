namespace DcodePe.Catering.Application.DataBase.Ticket.Commands.Create
{
    public class CreateTicketModel
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
        public string UsuarioCreacion { get; set; } = "SYSTEM";
    }
}
