using DcodePe.Catering.Application.ConsultaDocumento;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/consulta-documento")]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class ConsultaDocumentoController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("ruc/{numero}")]
        public async Task<IActionResult> ValidarRuc(
            [FromServices] IConsultaDocumentoService service,
            string numero)
        {
            var data = await service.ValidateRucAsync(numero);
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta RUC"));
        }

        [AllowAnonymous]
        [HttpGet("dni/{numero}")]
        public async Task<IActionResult> ValidarDni(
            [FromServices] IConsultaDocumentoService service,
            string numero)
        {
            var data = await service.ValidateDniAsync(numero);
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta DNI"));
        }
    }
}
