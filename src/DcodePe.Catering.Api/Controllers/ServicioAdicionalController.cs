//#if DEBUG // ?? Solo disponible en DESARROLLO
using DcodePe.Catering.Application.DataBase.ServicioAdicional.Commands.Create;
using DcodePe.Catering.Application.DataBase.ServicioAdicional.Commands.Delete;
using DcodePe.Catering.Application.DataBase.ServicioAdicional.Commands.Update;
using DcodePe.Catering.Application.DataBase.ServicioAdicional.Queries.GetAllServicioAdicional;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class ServicioAdicionalController : ControllerBase
    {

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromServices] ICreateServicioAdicionalCommand createServicioAdicionalCommand,
            [FromBody] CreateServicioAdicionalModel model)
        {
            var data = await createServicioAdicionalCommand.ExecuteSaveServicioAdicional(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Servicio adicional creado exitosamente"));
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateServicioAdicionalCommand updateServicioAdicionalCommand,
            [FromBody] UpdateServicioAdicionalModel model)
        {
            var result = await updateServicioAdicionalCommand.Execute(model);

            if (!result)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Servicio adicional no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, result, "Servicio adicional actualizado exitosamente"));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteServicioAdicionalCommand deleteServicioAdicionalCommand,
            int id,
            [FromQuery] string usuarioEliminacion = "SYSTEM")
        {
            var result = await deleteServicioAdicionalCommand.Execute(id, usuarioEliminacion);

            if (!result)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Servicio adicional no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, result, "Servicio adicional eliminado exitosamente"));
        }

        [HttpGet("getall")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetServicioAdicionalQuery getServicioAdicionalQuery)
        {
            var data = await getServicioAdicionalQuery.GetAllServicioAdicionalAsync();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("getbyid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            int id,
            [FromServices] IGetServicioAdicionalQuery getServicioAdicionalQuery)
        {
            var data = await getServicioAdicionalQuery.GetServicioAdicionalByIdAsync(id);

            if (data == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Servicio adicional no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }
    }
}

//#endif