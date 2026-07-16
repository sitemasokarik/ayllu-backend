namespace DcodePe.Catering.Domain.Entities.Tickets;

public class TicketVistoEntity
{
    public int TicketVistoID { get; set; }
    public int TicketID { get; set; }
    public int UsuarioID { get; set; }
    public DateTime FechaVisto { get; set; }
}
