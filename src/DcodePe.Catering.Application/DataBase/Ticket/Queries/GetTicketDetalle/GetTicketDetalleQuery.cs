using Microsoft.EntityFrameworkCore;



namespace DcodePe.Catering.Application.DataBase.Ticket.Queries.GetTicketDetalle

{

    public class GetTicketDetalleQuery(IDataBaseService databaseService) : IGetTicketDetalleQuery

    {

        private readonly IDataBaseService _databaseService = databaseService;



        public async Task<GetTicketDetalleModel?> Execute(int ticketId, int? clienteId = null, bool includeInternos = false)

        {

            var ticket = await _databaseService.TicketInterno

                .Include(t => t.Mensajes)

                .FirstOrDefaultAsync(t => t.TicketID == ticketId && t.Estado == true);



            if (ticket == null)

                return null;



            if (clienteId.HasValue && ticket.CreadoPorClienteID != clienteId.Value)

                return null;



            string? clienteNombre = null;

            string? clienteEmail = null;

            if (ticket.CreadoPorClienteID.HasValue)

            {

                var cliente = await _databaseService.Cliente.AsNoTracking()

                    .FirstOrDefaultAsync(c => c.ClienteID == ticket.CreadoPorClienteID.Value);

                clienteNombre = cliente?.NombreCompleto;

                clienteEmail = cliente?.Email;

            }



            string? creadoPorUsuarioNombre = null;

            if (ticket.CreadoPorUsuarioID.HasValue)

            {

                creadoPorUsuarioNombre = await _databaseService.Usuario.AsNoTracking()

                    .Where(u => u.UsuarioID == ticket.CreadoPorUsuarioID.Value)

                    .Select(u => u.Nombre)

                    .FirstOrDefaultAsync();

            }



            var mensajesQuery = ticket.Mensajes.Where(m => m.Estado == true);

            if (!includeInternos)

                mensajesQuery = mensajesQuery.Where(m => !m.EsInterno);



            var origen = ticket.CreadoPorClienteID.HasValue

                ? "Cliente"

                : ticket.CreadoPorUsuarioID.HasValue

                    ? "Usuario interno"

                    : "Sistema";



            return new GetTicketDetalleModel

            {

                TicketID = ticket.TicketID,

                Titulo = ticket.Titulo,

                Descripcion = ticket.Descripcion,

                EstadoTicket = ticket.EstadoTicket,

                Prioridad = ticket.Prioridad,

                CotizacionID = ticket.CotizacionID,

                RolDestinoID = ticket.RolDestinoID,

                CreadoPorClienteID = ticket.CreadoPorClienteID,

                CreadoPorUsuarioID = ticket.CreadoPorUsuarioID,

                ClienteNombre = clienteNombre,

                ClienteEmail = clienteEmail,

                CreadoPorUsuarioNombre = creadoPorUsuarioNombre,

                Origen = origen,

                FechaCreacion = ticket.FechaCreacion,

                Mensajes = mensajesQuery

                    .OrderBy(m => m.FechaCreacion)

                    .Select(m => new TicketMensajePortalModel

                    {

                        TicketMensajeID = m.TicketMensajeID,

                        AutorNombre = m.AutorNombre,

                        Mensaje = m.Mensaje,

                        FechaCreacion = m.FechaCreacion,

                        EsCliente = m.ClienteID.HasValue,

                        EsInterno = m.EsInterno

                    })

                    .ToList()

            };

        }

    }

}


