using DcodePe.Catering.Application.DataBase.PagoVoucher.Commands.Review;
using DcodePe.Catering.Application.DataBase.PagoVoucher.Queries.GetHistorial;
using DcodePe.Catering.Application.DataBase.PagoVoucher.Queries.GetPendientes;
using DcodePe.Catering.Application.Database;
using DcodePe.Catering.Api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [Route("api/v1/pago-voucher")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class PagoVoucherController(IWebHostEnvironment environment) : ControllerBase
    {
        private readonly IWebHostEnvironment _environment = environment;

        [HttpGet("pendientes")]
        public async Task<IActionResult> GetPendientes([FromServices] IGetPagoVoucherPendientesQuery query)
        {
            var data = await query.Execute();
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("historial")]
        public async Task<IActionResult> GetHistorial(
            [FromServices] IGetPagoVoucherHistorialQuery query,
            [FromQuery] string? estadoPago = null)
        {
            var data = await query.Execute(estadoPago);
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("pendientes/count")]
        public async Task<IActionResult> CountPendientes([FromServices] IGetPagoVoucherPendientesQuery query)
        {
            var count = await query.CountPendientesAsync();
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, new { count }, "Consulta exitosa"));
        }

        [HttpGet("{id}/archivo")]
        public async Task<IActionResult> DescargarArchivo(
            int id,
            [FromServices] IDataBaseService db,
            [FromQuery] bool inline = false)
        {
            var voucher = await db.PagoVoucher
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.PagoVoucherID == id && v.Estado == true);

            if (voucher == null || string.IsNullOrWhiteSpace(voucher.ArchivoUrl))
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Voucher no encontrado"));

            var physicalPath = UploadFileHelper.ResolvePhysicalPath(_environment, voucher.ArchivoUrl);
            if (!System.IO.File.Exists(physicalPath))
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Archivo no disponible en el servidor"));

            var contentType = UploadFileHelper.GetContentType(voucher.ArchivoUrl);
            var fileName = string.IsNullOrWhiteSpace(voucher.NombreArchivo)
                ? Path.GetFileName(physicalPath)
                : voucher.NombreArchivo;

            if (inline)
                return PhysicalFile(physicalPath, contentType);

            return PhysicalFile(physicalPath, contentType, fileName);
        }

        [HttpPut("revisar")]
        public async Task<IActionResult> Revisar(
            [FromServices] IReviewPagoVoucherCommand command,
            [FromBody] ReviewPagoVoucherModel model)
        {
            try
            {
                var ok = await command.Execute(model);
                if (!ok)
                    return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Voucher no encontrado"));

                return Ok(ResponseApiService.Response(StatusCodes.Status200OK, true,
                    model.Aprobado ? "Pago aprobado. Cotización confirmada como Evento." : "Pago rechazado."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }
    }
}
