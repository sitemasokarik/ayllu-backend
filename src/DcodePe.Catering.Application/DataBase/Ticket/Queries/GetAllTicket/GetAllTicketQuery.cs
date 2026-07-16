using Microsoft.EntityFrameworkCore;



namespace DcodePe.Catering.Application.DataBase.Ticket.Queries.GetAllTicket

{

    public class GetAllTicketQuery(IDataBaseService databaseService) : IGetAllTicketQuery

    {

        private readonly IDataBaseService _databaseService = databaseService;



        public async Task<List<GetAllTicketModel>> Execute(int? usuarioId = null, int? clienteId = null)

        {

            var query = _databaseService.TicketInterno

                .Where(t => t.Estado == true);



            if (usuarioId.HasValue)

            {

                query = query.Where(t =>

                    t.CreadoPorUsuarioID == usuarioId.Value ||

                    t.AsignadoUsuarioID == usuarioId.Value);

            }



            if (clienteId.HasValue)

            {

                query = query.Where(t => t.CreadoPorClienteID == clienteId.Value);

            }



            var tickets = await query

                .OrderByDescending(t => t.TicketID)

                .Select(t => new

                {

                    t.TicketID,

                    t.Titulo,

                    t.Descripcion,

                    t.EstadoTicket,

                    t.Prioridad,

                    t.CreadoPorUsuarioID,

                    t.AsignadoUsuarioID,

                    t.CreadoPorClienteID,

                    t.RolDestinoID,

                    t.CotizacionID,

                    TotalMensajes = t.Mensajes.Count(m => m.Estado == true),

                    t.FechaCreacion

                })

                .ToListAsync();



            var clienteIds = tickets

                .Where(t => t.CreadoPorClienteID.HasValue)

                .Select(t => t.CreadoPorClienteID!.Value)

                .Distinct()

                .ToList();



            var clientes = clienteIds.Count == 0

                ? new Dictionary<int, (string Nombre, string Email)>()

                : await _databaseService.Cliente.AsNoTracking()

                    .Where(c => clienteIds.Contains(c.ClienteID))

                    .Select(c => new { c.ClienteID, c.NombreCompleto, c.Email })

                    .ToDictionaryAsync(c => c.ClienteID, c => (Nombre: c.NombreCompleto, Email: c.Email ?? string.Empty));



            var asignadoIds = tickets

                .Where(t => t.AsignadoUsuarioID.HasValue)

                .Select(t => t.AsignadoUsuarioID!.Value)

                .Distinct()

                .ToList();



            var usuarios = asignadoIds.Count == 0

                ? new Dictionary<int, string>()

                : await _databaseService.Usuario.AsNoTracking()

                    .Where(u => asignadoIds.Contains(u.UsuarioID))

                    .Select(u => new { u.UsuarioID, u.Nombre })

                    .ToDictionaryAsync(u => u.UsuarioID, u => u.Nombre ?? string.Empty);



            return tickets.Select(t =>

            {

                clientes.TryGetValue(t.CreadoPorClienteID ?? 0, out var cliente);

                usuarios.TryGetValue(t.AsignadoUsuarioID ?? 0, out var asignadoNombre);

                return new GetAllTicketModel

                {

                    TicketID = t.TicketID,

                    Titulo = t.Titulo,

                    Descripcion = t.Descripcion,

                    EstadoTicket = t.EstadoTicket,

                    Prioridad = t.Prioridad,

                    CreadoPorUsuarioID = t.CreadoPorUsuarioID,

                    AsignadoUsuarioID = t.AsignadoUsuarioID,

                    CreadoPorClienteID = t.CreadoPorClienteID,

                    RolDestinoID = t.RolDestinoID,

                    CotizacionID = t.CotizacionID,

                    TotalMensajes = t.TotalMensajes,

                    ClienteNombre = t.CreadoPorClienteID.HasValue ? cliente.Nombre : null,

                    ClienteEmail = t.CreadoPorClienteID.HasValue ? cliente.Email : null,

                    AsignadoUsuarioNombre = t.AsignadoUsuarioID.HasValue ? asignadoNombre : null,

                    Origen = t.CreadoPorClienteID.HasValue ? "Cliente" : t.CreadoPorUsuarioID.HasValue ? "Usuario interno" : "Sistema",

                    FechaCreacion = t.FechaCreacion

                };

            }).ToList();

        }

    }

}


