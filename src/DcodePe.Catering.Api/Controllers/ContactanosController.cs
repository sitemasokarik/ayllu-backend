//#if DEBUG
using DcodePe.Catering.Application.DataBase.Contactanos.Commands.Create;
using DcodePe.Catering.Application.DataBase.Contactanos.Queries.GetAllContactanos;

namespace DcodePe.Catering.Api.Controllers
{
    [Route("api/v1/contactanos")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class ContactanosController : ControllerBase
    {
        [HttpPost("create")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateContactanosModel model,
            [FromServices] ICreateContactanosCommand createContactanosCommand)
        {
            var data = await createContactanosCommand.Execute(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Mensaje de contacto enviado exitosamente"));
        }

        [HttpGet("getall")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllContactanosQuery getAllContactanosQuery)
        {
            var data = await getAllContactanosQuery.ExecuteListContactanos();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("getbyid/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            int id,
            [FromServices] IGetAllContactanosQuery getAllContactanosQuery)
        {
            var data = await getAllContactanosQuery.ExecuteGetContactanosById(id);

            if (data == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Mensaje de contacto no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }
    }
}

//#endif
