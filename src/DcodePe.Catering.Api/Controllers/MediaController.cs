using DcodePe.Catering.Api.Helpers;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [Route("api/v1/media")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class MediaController : ControllerBase
    {
        private static readonly Dictionary<string, string> AllowedFolders = new(StringComparer.OrdinalIgnoreCase)
        {
            ["productos"] = "uploads/productos",
            ["locales"] = "uploads/locales",
            ["servicios"] = "uploads/servicios",
            ["eventos"] = "uploads/eventos",
            ["blog"] = "uploads/blog",
            ["empresa"] = "uploads/empresa"
        };

        [HttpPost("upload")]
        [RequestSizeLimit(6 * 1024 * 1024)]
        public async Task<IActionResult> Upload(
            [FromQuery] string folder,
            [FromServices] IFileStorageService fileStorage,
            [FromForm] IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, null, "Debes seleccionar una imagen."));

            if (string.IsNullOrWhiteSpace(folder) || !AllowedFolders.TryGetValue(folder.Trim(), out var virtualFolder))
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, null,
                    "Carpeta no válida. Usa: productos, locales, servicios, eventos, blog o empresa."));

            try
            {
                var url = await fileStorage.SaveImageAsync(archivo, virtualFolder);
                return Ok(ResponseApiService.Response(StatusCodes.Status200OK, new { url }, "Imagen subida correctamente."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }
    }
}
