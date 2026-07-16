using DcodePe.Catering.Application.External.Sunat;
using DcodePe.Catering.Application.Database;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Api.Controllers
{
    public partial class ComprobanteController
    {
        [HttpPost("{id}/reenviar-sunat")]
        public async Task<IActionResult> ReenviarSunat(
            int id,
            [FromServices] IDataBaseService db,
            [FromServices] ISunatEmisionService sunatEmisionService)
        {
            var comprobante = await db.ComprobanteElectronico
                .FirstOrDefaultAsync(c => c.ComprobanteID == id && c.Estado == true);

            if (comprobante == null)
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Comprobante no encontrado"));

            comprobante.Detalles = await db.ComprobanteDetalle
                .Where(d => d.ComprobanteID == id)
                .ToListAsync();

            var result = await sunatEmisionService.EmitirComprobanteAsync(comprobante);

            comprobante.SunatTicket = result.Ticket;
            comprobante.SunatRespuesta = result.Respuesta;
            comprobante.SunatCdr = result.SunatCdr;
            comprobante.SunatHashCpe = result.SunatHashCpe;
            comprobante.RutaXml = result.RutaXml;
            comprobante.RutaCdr = result.RutaCdr;
            comprobante.SunatCodigoError = result.SunatCodigoError;
            comprobante.EstadoComprobante = result.EstadoComprobante;

            await db.SaveAsync();

            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, new
            {
                comprobante.ComprobanteID,
                comprobante.EstadoComprobante,
                comprobante.SunatRespuesta,
                comprobante.SunatCodigoError
            }, "Reenvío procesado"));
        }

        [HttpGet("{id}/descargar")]
        public async Task<IActionResult> Descargar(
            int id,
            [FromServices] IDataBaseService db,
            [FromQuery] string tipo = "xml")
        {
            var comprobante = await db.ComprobanteElectronico
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ComprobanteID == id && c.Estado == true);

            if (comprobante == null)
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Comprobante no encontrado"));

            var relativePath = tipo.Equals("cdr", StringComparison.OrdinalIgnoreCase)
                ? comprobante.RutaCdr
                : comprobante.RutaXml;

            if (string.IsNullOrWhiteSpace(relativePath))
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Archivo no disponible"));

            var physicalPath = Path.Combine(AppContext.BaseDirectory, relativePath.Replace('/', Path.DirectorySeparatorChar));
            if (!System.IO.File.Exists(physicalPath))
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Archivo no encontrado en disco"));

            var contentType = tipo.Equals("cdr", StringComparison.OrdinalIgnoreCase)
                ? "application/zip"
                : "application/xml";

            return PhysicalFile(physicalPath, contentType, Path.GetFileName(physicalPath));
        }
    }
}
