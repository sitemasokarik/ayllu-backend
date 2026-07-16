using DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Claim;
using DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Create;
using DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Update;
using DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetAllCotizacion;
using DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetCotizacionFacturacion;
using DcodePe.Catering.Application.Database;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/cotizacion")]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class CotizacionController : ControllerBase
    {
        [HttpPost("create")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromServices] ICreateCotizacionCommand createCotizacionCommand,
            [FromServices] IValidator<CreateCotizacionModel> validator,
            [FromBody] CreateCotizacionModel model)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            var data = await createCotizacionCommand.ExecuteSaveCotizacion(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Cotización creada exitosamente"));
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateCotizacionCommand updateCotizacionCommand,
            [FromServices] IValidator<UpdateCotizacionModel> validator,
            [FromBody] UpdateCotizacionModel model)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            var result = await updateCotizacionCommand.Execute(model);

            if (!result)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Cotización no encontrada"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, result, "Cotización actualizada exitosamente"));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteCotizacionCommand deleteCotizacionCommand,
            int id,
            [FromQuery] string usuarioEliminacion = "SYSTEM")
        {
            var result = await deleteCotizacionCommand.Execute(id, usuarioEliminacion);

            if (!result)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Cotización no encontrada"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, result, "Cotización eliminada exitosamente"));
        }

        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllCotizacionQuery getAllCotizacionQuery)
        {
            var data = await getAllCotizacionQuery.ExecuteListaCotizacion();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("count/recientes")]
        public async Task<IActionResult> CountRecientes(
            [FromServices] IDataBaseService db,
            [FromQuery] DateTime? vistoDesde = null)
        {
            var desde = DateTime.Now.AddHours(-48);
            var query = db.Cotizacion.Where(c =>
                c.Estado == true
                && c.EstadoCotizacion == "Activo"
                && c.FechaCreacion >= desde);

            if (vistoDesde.HasValue)
                query = query.Where(c => c.FechaCreacion > vistoDesde.Value);

            var count = await query.CountAsync();

            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, new { count }, "Consulta exitosa"));
        }

        [HttpPost("{id}/tomar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Tomar(
            int id,
            [FromServices] IClaimCotizacionCommand claimCotizacionCommand,
            [FromQuery] int usuarioId,
            [FromQuery] string usuarioNombre = "")
        {
            if (usuarioId <= 0)
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, null, "usuarioId requerido"));

            var result = await claimCotizacionCommand.Execute(id, usuarioId, usuarioNombre);

            if (!result.Success && result.Message == "Cotización no encontrada.")
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, result.Message));

            if (!result.Success)
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, result, result.Message));

            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, result, result.Message));
        }

        [HttpGet("getbyid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromServices] IGetAllCotizacionQuery getAllCotizacionQuery,
            int id)
        {
            var data = await getAllCotizacionQuery.ExecuteListaCotizacionById(id);

            if (data == null || !data.Any())
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Cotización no encontrada"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("{id}/facturacion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFacturacion(
            int id,
            [FromServices] IGetCotizacionFacturacionQuery query)
        {
            try
            {
                var data = await query.Execute(id);
                return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ResponseApiService.Response(StatusCodes.Status500InternalServerError, null, ex.Message));
            }
        }
    }
}
//#endif
