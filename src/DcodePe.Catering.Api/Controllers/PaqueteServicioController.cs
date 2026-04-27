#if DEBUG // ?? Solo disponible en DESARROLLO
namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApiAll)]
    [TypeFilter(typeof(ExceptionManager))]
    public class PaqueteServicioController : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromServices] ICreatePaqueteServicioCommand createPaqueteServicioCommand, [FromBody] CreatePaqueteServicioModel model)
        {
            var data = await createPaqueteServicioCommand.CreatePaqueteServicio(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Registro exitoso"));
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromServices] IGetAllPaqueteServicioQuery getAllPaqueteServicioQuery)
        {
            var data = await getAllPaqueteServicioQuery.GetAllPaqueteServicio();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }
    }
}

#endif