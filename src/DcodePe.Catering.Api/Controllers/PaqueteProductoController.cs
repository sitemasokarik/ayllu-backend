#if DEBUG // ?? Solo disponible en DESARROLLO
using DcodePe.Catering.Application.DataBase.PaqueteProducto.Commands.Create;
using DcodePe.Catering.Application.DataBase.PaqueteProducto.Queries.GetAllPaqueteProducto;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IgnoreApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class PaqueteProductoController : ControllerBase
    {

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromServices] ICreatePaqueteProductoCommand createPaqueteProductoCommand, [FromBody] CreatePaqueteProductoModel model)
        {
            var data = await createPaqueteProductoCommand.ExecuteSavePaqueteProducto(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Registro exitoso"));
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromServices] IGetAllPaqueteProductoQuery getAllPaqueteProductoQuery)
        {
            var data = await getAllPaqueteProductoQuery.ExecuteGetAllPaqueteProducto();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

    }
}

#endif