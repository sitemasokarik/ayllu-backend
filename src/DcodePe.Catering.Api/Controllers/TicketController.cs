using DcodePe.Catering.Application.DataBase.Ticket.Commands.AddMensaje;
using DcodePe.Catering.Application.DataBase.Ticket.Commands.Create;
using DcodePe.Catering.Application.DataBase.Ticket.Commands.UpdateEstado;
using DcodePe.Catering.Application.DataBase.Ticket.Queries.GetAllTicket;
using DcodePe.Catering.Application.DataBase.Ticket.Queries.GetTicketDetalle;
using DcodePe.Catering.Application.Database;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/ticket")]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class TicketController : ControllerBase
    {
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllTicketQuery query,
            [FromQuery] int? usuarioId = null)
        {
            var data = await query.Execute(usuarioId);
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(
            int id,
            [FromServices] IGetTicketDetalleQuery query,
            [FromServices] IDataBaseService db,
            [FromQuery] int? usuarioId = null)
        {
            var data = await query.Execute(id, includeInternos: true);
            if (data == null)
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Ticket no encontrado"));

            if (usuarioId.HasValue)
                await MarcarVistoAsync(db, id, usuarioId.Value);

            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("count/alertas")]
        public async Task<IActionResult> CountAlertas(
            [FromServices] IDataBaseService db,
            [FromQuery] int? usuarioId = null)
        {
            var vistos = usuarioId.HasValue
                ? await db.TicketVisto.AsNoTracking()
                    .Where(v => v.UsuarioID == usuarioId.Value)
                    .Select(v => v.TicketID)
                    .ToListAsync()
                : new List<int>();

            var count = await db.TicketInterno.CountAsync(t =>
                t.Estado == true
                && t.EstadoTicket != "Cerrado"
                && !vistos.Contains(t.TicketID)
                && (
                    t.CreadoPorClienteID != null
                    || (usuarioId.HasValue && t.AsignadoUsuarioID == usuarioId.Value)
                ));

            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, new { count }, "Consulta exitosa"));
        }

        [HttpPost("{id}/marcar-visto")]
        public async Task<IActionResult> MarcarVisto(
            int id,
            [FromServices] IDataBaseService db,
            [FromQuery] int usuarioId)
        {
            if (usuarioId <= 0)
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, null, "usuarioId requerido"));

            await MarcarVistoAsync(db, id, usuarioId);
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, true, "Ticket marcado como visto"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(
            [FromServices] ICreateTicketCommand command,
            [FromBody] CreateTicketModel model)
        {
            var data = await command.Execute(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Ticket creado"));
        }

        [HttpPost("mensaje")]
        public async Task<IActionResult> AddMensaje(
            [FromServices] IAddTicketMensajeCommand command,
            [FromBody] AddTicketMensajeModel model)
        {
            var data = await command.Execute(model);
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Mensaje agregado"));
        }

        [HttpPut("estado")]
        public async Task<IActionResult> UpdateEstado(
            [FromServices] IUpdateTicketEstadoCommand command,
            [FromBody] UpdateTicketEstadoModel model)
        {
            var result = await command.Execute(model);
            if (result == null)
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Ticket no encontrado"));
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, result, "Estado actualizado"));
        }

        private static async Task MarcarVistoAsync(IDataBaseService db, int ticketId, int usuarioId)
        {
            var exists = await db.TicketVisto.AnyAsync(v =>
                v.TicketID == ticketId && v.UsuarioID == usuarioId);

            if (exists)
                return;

            await db.TicketVisto.AddAsync(new Domain.Entities.Tickets.TicketVistoEntity
            {
                TicketID = ticketId,
                UsuarioID = usuarioId,
                FechaVisto = DateTime.Now
            });
            await db.SaveAsync();
        }
    }
}
