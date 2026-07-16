namespace DcodePe.Catering.Application.DataBase.Ticket.Queries.GetTicketDetalle

{

    public class TicketMensajePortalModel

    {

        public int TicketMensajeID { get; set; }

        public string AutorNombre { get; set; } = string.Empty;

        public string Mensaje { get; set; } = string.Empty;

        public DateTime? FechaCreacion { get; set; }

        public bool EsCliente { get; set; }

        public bool EsInterno { get; set; }

    }



    public class GetTicketDetalleModel

    {

        public int TicketID { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public string EstadoTicket { get; set; } = string.Empty;

        public string Prioridad { get; set; } = string.Empty;

        public int? CotizacionID { get; set; }

        public int? RolDestinoID { get; set; }

        public int? CreadoPorClienteID { get; set; }

        public int? CreadoPorUsuarioID { get; set; }

        public string? ClienteNombre { get; set; }

        public string? ClienteEmail { get; set; }

        public string? CreadoPorUsuarioNombre { get; set; }

        public string Origen { get; set; } = string.Empty;

        public DateTime? FechaCreacion { get; set; }

        public List<TicketMensajePortalModel> Mensajes { get; set; } = [];

    }



    public interface IGetTicketDetalleQuery

    {

        Task<GetTicketDetalleModel?> Execute(int ticketId, int? clienteId = null, bool includeInternos = false);

    }

}


