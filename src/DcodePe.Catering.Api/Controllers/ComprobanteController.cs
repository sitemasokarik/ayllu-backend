using DcodePe.Catering.Application.ConsultaDocumento;
using DcodePe.Catering.Application.DataBase.Comprobante.Commands.Create;
using DcodePe.Catering.Application.DataBase.Comprobante.Queries.GetAllComprobante;
using DcodePe.Catering.Application.DataBase.Comprobante.Queries.GetComprobanteById;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/comprobante")]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public partial class ComprobanteController : ControllerBase
    {
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllComprobanteQuery query,
            [FromQuery] string? tipo = null)
        {
            var data = await query.Execute(tipo);
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            [FromServices] IGetComprobanteByIdQuery query,
            int id)
        {
            var data = await query.Execute(id);
            if (data == null)
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Comprobante no encontrado"));
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpPost("emitir")]
        public async Task<IActionResult> Emitir(
            [FromServices] ICreateComprobanteCommand command,
            [FromBody] CreateComprobanteModel model)
        {
            var data = await command.Execute(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Comprobante registrado"));
        }
    }
}
