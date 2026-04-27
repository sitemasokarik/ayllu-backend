//#if DEBUG
using DcodePe.Catering.Application.DataBase.Evento.Commands.Create;
using DcodePe.Catering.Application.DataBase.Evento.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Evento.Commands.Update;
using DcodePe.Catering.Application.DataBase.Evento.Queries.GetAllEvento;
using DcodePe.Catering.Application.DataBase.Evento.Queries.GetById;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [Route("api/v1/evento")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class EventoController : ControllerBase
    {
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateEventoModel model,
            [FromServices] ICreateEventoCommand createEventoCommand)
        {
            var data = await createEventoCommand.ExecuteSaveEvento(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Evento creado exitosamente"));
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateEventoCommand updateEventoCommand,
            [FromBody] UpdateEventoModel model)
        {
            var result = await updateEventoCommand.Execute(model);

            if (!result)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Evento no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, result, "Evento actualizado exitosamente"));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteEventoCommand deleteEventoCommand,
            int id,
            [FromQuery] string usuarioEliminacion = "SYSTEM")
        {
            var result = await deleteEventoCommand.Execute(id, usuarioEliminacion);

            if (!result)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Evento no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, result, "Evento eliminado exitosamente"));
        }

        [HttpGet("getall")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllEventoQuery getAllEventoQuery)
        {
            var data = await getAllEventoQuery.ExecuteListEvento();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        /// <summary>
        /// Obtiene un evento por su ID, incluyendo el total de cotizaciones asociadas
        /// </summary>
        /// <param name="id">ID del evento a buscar</param>
        /// <returns>Evento con información completa</returns>
        [HttpGet("getbyid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromServices] IGetEventoByIdQuery getEventoByIdQuery,
            int id)
        {
            var data = await getEventoByIdQuery.Execute(id);

            if (data == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Evento no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }
    }
}

//#endif
