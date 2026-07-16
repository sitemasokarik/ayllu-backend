namespace DcodePe.Catering.Application.DataBase.Ticket.Commands.UpdateEstado
{
    public class UpdateTicketEstadoModel
    {
        public int TicketID { get; set; }
        public string EstadoTicket { get; set; } = string.Empty;
        public string UsuarioModificacion { get; set; } = "SYSTEM";
    }
}
