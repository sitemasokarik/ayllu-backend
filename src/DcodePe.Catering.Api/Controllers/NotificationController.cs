#if DEBUG // ?? Solo disponible en DESARROLLO
namespace DcodePe.Catering.Api.Controllers
{
    [Route("api/v1/notification")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApiAll)]
    [TypeFilter(typeof(ExceptionManager))]
    public class NotificationController : ControllerBase
    {
        [HttpPost("create")]

        public async Task<IActionResult> Create(
            [FromBody] MailerSendEmailRequestModel model,
            [FromServices] IMailerSendEmailService mailerSendEmailService)
        {
      
            var data = await mailerSendEmailService.Execute(model);


            if(!data)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ResponseApiService.Response(StatusCodes.Status500InternalServerError,
                    data, "Error al enviar el correo"));

            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Correo enviado con exito"));
        }
    }
}

#endif