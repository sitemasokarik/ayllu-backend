//#if DEBUG // ?? Solo disponible en DESARROLLO
using DcodePe.Catering.Application.DataBase.Local.Commands.CreateLocal;
using DcodePe.Catering.Application.DataBase.Local.Commands.Update;
using DcodePe.Catering.Application.DataBase.Local.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Local.Queries.GetAllLocal;
using DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetFechasReservadasLocal;

namespace DcodePe.Catering.Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de locales de eventos
    /// </summary>
    [Authorize]
    [Route("api/v1/local")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class LocalController : ControllerBase
    {
        /// <summary>
        /// Crea un nuevo local
        /// </summary>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateLocalModel model,
            [FromServices] ICreateLocalCommand createLocalCommand)
        {
            var data = await createLocalCommand.ExecuteSaveLocal(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Local creado exitosamente"));
        }

        /// <summary>
        /// Actualiza un local existente
        /// </summary>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateLocalCommand updateLocalCommand,
            [FromBody] UpdateLocalModel model)
        {
            var result = await updateLocalCommand.Execute(model);

            if (!result)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Local no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, result, "Local actualizado exitosamente"));
        }

        /// <summary>
        /// Elimina un local (soft delete)
        /// </summary>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteLocalCommand deleteLocalCommand,
            int id,
            [FromQuery] string usuarioEliminacion = "SYSTEM")
        {
            try
            {
                var result = await deleteLocalCommand.Execute(id, usuarioEliminacion);

                if (!result)
                    return StatusCode(StatusCodes.Status404NotFound,
                        ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Local no encontrado"));

                return StatusCode(StatusCodes.Status200OK,
                    ResponseApiService.Response(StatusCodes.Status200OK, result, "Local eliminado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }

        /// <summary>
        /// Obtiene todos los locales activos
        /// </summary>
        [HttpGet("getall")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromServices] IGetAllLocalQuery getAllLocalQuery)
        {
            var data = await getAllLocalQuery.GetAllLocals();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        /// <summary>
        /// Obtiene un local por su ID
        /// </summary>
        [HttpGet("getbyid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            int id, 
            [FromServices] IGetAllLocalQuery getAllLocalQuery)
        {
            var data = await getAllLocalQuery.GetAllLocalsById(id);
            
            if (data == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Local no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("{id}/fechas-reservadas")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFechasReservadas(
            int id,
            [FromServices] IGetFechasReservadasLocalQuery query)
        {
            var data = await query.Execute(id);
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }
    }
}