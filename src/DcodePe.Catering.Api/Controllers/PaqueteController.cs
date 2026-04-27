#if DEBUG // ?? Solo disponible en DESARROLLO
using DcodePe.Catering.Application.DataBase.Paquete.Commands.Create;
using DcodePe.Catering.Application.DataBase.Paquete.Queries.GetAllPaquete;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IgnoreApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class PaqueteController : Controller
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromServices] ICreatePaqueteCommand createPaqueteCommand, [FromBody] CreatePaqueteModel model)
        {
            var data = await createPaqueteCommand.ExecuteSavePaquete(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Registro exitoso"));
        }

        /// <summary>
        /// Crea un paquete completo con múltiples productos y servicios en una sola transacción
        /// </summary>
        [HttpPost("createproductosservices")]
        public async Task<IActionResult> CreateConProductos(
            [FromServices] ICreatePaqueteConProductosCommand command,
            [FromServices] IValidator<CreatePaqueteProductosModel> validator,
            [FromBody] CreatePaqueteProductosModel model)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            var data = await command.ExecuteSavePaqueteConProductos(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Paquete creado con productos y servicios exitosamente"));
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllPaquete([FromServices] IGetAllPaqueteQuery getAllPaqueteQuery)
        {
            var data = await getAllPaqueteQuery.ExecuteGetAllPaquete();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("getpaquetewithproducto")]
        public async Task<IActionResult> GetPaqueteWithProductos([FromServices] IGetAllPaqueteQuery getAllPaqueteQuery)
        {
            var data = await getAllPaqueteQuery.ExecuteGetPaqueteWithProductos();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("getpaquetewithproductobyid/{paqueteId}")]
        public async Task<IActionResult> GetPaqueteWithProductosById([FromServices] IGetAllPaqueteQuery getAllPaqueteQuery, int paqueteId)
        {
            var data = await getAllPaqueteQuery.ExecuteGetPaqueteWithProductosByIdPaquete(paqueteId);
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("getpaquetewithservicio")]
        public async Task<IActionResult> GetPaqueteWithServicios([FromServices] IGetAllPaqueteQuery getAllPaqueteQuery)
        {
            var data = await getAllPaqueteQuery.ExecuteGetPaqueteWithServicios();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("getpaquetewithservicio/{paqueteId}")]
        public async Task<IActionResult> GetPaqueteWithServicioById([FromServices] IGetAllPaqueteQuery getAllPaqueteQuery, int paqueteId)
        {
            var data = await getAllPaqueteQuery.ExecuteGetPaqueteWithServicioByIdPaquete(paqueteId);
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("getallpaqueteproductosandservicios")]
        public async Task<IActionResult> GetAllPaqueteProductosAndServicios([FromServices] IGetAllPaqueteQuery getAllPaqueteQuery)
        {
            var data = await getAllPaqueteQuery.ExecuteGetAllPaqueteWithProductosAndServicios();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("getallpaquetewithproductosandserviciosbyId/{paqueteId}")]
        public async Task<IActionResult> GetAllPaqueteProductosAndServicios([FromServices] IGetAllPaqueteQuery getAllPaqueteQuery, int paqueteId)
        {
            var data = await getAllPaqueteQuery.ExecuteGetAllPaqueteWithProductosAndServiciosById(paqueteId);
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }
    }
}

#endif