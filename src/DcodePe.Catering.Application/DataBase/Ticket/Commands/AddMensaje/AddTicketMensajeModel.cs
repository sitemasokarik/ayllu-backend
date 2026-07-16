namespace DcodePe.Catering.Application.DataBase.Ticket.Commands.AddMensaje
{
    public class AddTicketMensajeModel
    {
        public int TicketMensajeID { get; set; }
        public int TicketID { get; set; }
        public int? UsuarioID { get; set; }
        public int? ClienteID { get; set; }
        public string AutorNombre { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public bool EsInterno { get; set; }
        public string UsuarioCreacion { get; set; } = "SYSTEM";
    }
}
