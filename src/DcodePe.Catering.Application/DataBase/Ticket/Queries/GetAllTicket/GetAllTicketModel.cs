namespace DcodePe.Catering.Application.DataBase.Ticket.Queries.GetAllTicket
{
    public class GetAllTicketModel
    {
        public int TicketID { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string EstadoTicket { get; set; } = string.Empty;
        public string Prioridad { get; set; } = string.Empty;
        public int? CreadoPorUsuarioID { get; set; }
        public int? AsignadoUsuarioID { get; set; }
        public int? CreadoPorClienteID { get; set; }
        public int? RolDestinoID { get; set; }
        public int? CotizacionID { get; set; }
        public int TotalMensajes { get; set; }
        public string? ClienteNombre { get; set; }
        public string? ClienteEmail { get; set; }
        public string? AsignadoUsuarioNombre { get; set; }
        public string Origen { get; set; } = string.Empty;
        public DateTime? FechaCreacion { get; set; }
    }
}
