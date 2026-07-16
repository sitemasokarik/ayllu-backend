using DcodePe.Catering.Application.DataBase.MercadoPago.Commands.ProcessWebhook;
using System.Text.Json;

namespace DcodePe.Catering.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/v1/webhooks")]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    public class MercadoPagoWebhookController : ControllerBase
    {
        [HttpPost("mercadopago")]
        public async Task<IActionResult> MercadoPago(
            [FromServices] IProcessMercadoPagoWebhookCommand command,
            [FromQuery] string? topic,
            [FromQuery] string? id,
            [FromQuery(Name = "data.id")] string? dataId)
        {
            var resourceId = dataId ?? id;
            if (string.IsNullOrWhiteSpace(resourceId) && Request.ContentLength > 0)
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrWhiteSpace(body))
                {
                    using var doc = JsonDocument.Parse(body);
                    if (doc.RootElement.TryGetProperty("data", out var data)
                        && data.TryGetProperty("id", out var dataIdProp))
                    {
                        resourceId = dataIdProp.GetRawText().Trim('"');
                    }

                    if (doc.RootElement.TryGetProperty("type", out var typeProp))
                        topic = typeProp.GetString();
                }
            }

            Request.Headers.TryGetValue("x-signature", out var xSignature);
            Request.Headers.TryGetValue("x-request-id", out var xRequestId);

            await command.Execute(new MercadoPagoWebhookModel
            {
                Topic = topic,
                ResourceId = resourceId,
                XSignature = xSignature.ToString(),
                XRequestId = xRequestId.ToString()
            });

            return Ok();
        }
    }
}
