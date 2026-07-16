//#if DEBUG
using DcodePe.Catering.Application.DataBase.Empresa.Commands.Create;
using DcodePe.Catering.Application.DataBase.Empresa.Commands.Update;
using DcodePe.Catering.Application.DataBase.Empresa.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Empresa.Queries.GetAllEmpresa;
using DcodePe.Catering.Api.Helpers;
using DcodePe.Catering.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [Route("api/v1/empresa")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class EmpresaController : ControllerBase
    {
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateEmpresaModel model,
            [FromServices] ICreateEmpresaCommand createEmpresaCommand)
        {
            var data = await createEmpresaCommand.Execute(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Empresa creada exitosamente"));
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateEmpresaCommand updateEmpresaCommand,
            [FromBody] UpdateEmpresaModel model)
        {
            var result = await updateEmpresaCommand.Execute(model);

            if (!result)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Empresa no encontrada"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, result, "Empresa actualizada exitosamente"));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteEmpresaCommand deleteEmpresaCommand,
            int id,
            [FromQuery] string usuarioEliminacion = "SYSTEM")
        {
            var result = await deleteEmpresaCommand.Execute(id, usuarioEliminacion);

            if (!result)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Empresa no encontrada"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, result, "Empresa eliminada exitosamente"));
        }

        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllEmpresaQuery getAllEmpresaQuery)
        {
            var data = await getAllEmpresaQuery.ExecuteListEmpresa();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("getbyid/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            int id,
            [FromServices] IGetAllEmpresaQuery getAllEmpresaQuery)
        {
            var data = await getAllEmpresaQuery.ExecuteGetEmpresaById(id);

            if (data == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Empresa no encontrada"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("getactiva")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActiva(
            [FromServices] IGetAllEmpresaQuery getAllEmpresaQuery)
        {
            var data = await getAllEmpresaQuery.ExecuteGetEmpresaActiva();

            if (data == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "No hay empresa activa"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpPost("upload-qr-pago/{id}")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> UploadQrPago(
            int id,
            [FromServices] IFileStorageService fileStorage,
            [FromServices] IUpdateEmpresaCommand updateEmpresaCommand,
            [FromServices] IGetAllEmpresaQuery getAllEmpresaQuery,
            IFormFile archivo)
        {
            var empresa = await getAllEmpresaQuery.ExecuteGetEmpresaById(id);
            if (empresa == null)
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Empresa no encontrada"));

            try
            {
                var qrUrl = await fileStorage.SaveQrPagoAsync(archivo);
                var updated = await updateEmpresaCommand.Execute(new UpdateEmpresaModel
                {
                    EmpresaID = empresa.EmpresaID,
                    RazonSocial = empresa.RazonSocial,
                    NombreComercial = empresa.NombreComercial,
                    RUC = empresa.RUC,
                    Email = empresa.Email,
                    Telefono = empresa.Telefono,
                    TelefonoSecundario = empresa.TelefonoSecundario,
                    WhatsApp = empresa.WhatsApp,
                    Direccion = empresa.Direccion,
                    Ciudad = empresa.Ciudad,
                    Pais = empresa.Pais,
                    Facebook = empresa.Facebook,
                    Instagram = empresa.Instagram,
                    LinkedIn = empresa.LinkedIn,
                    Twitter = empresa.Twitter,
                    HorarioAtencion = empresa.HorarioAtencion,
                    Logo = empresa.Logo,
                    BancoNombre = empresa.BancoNombre,
                    NumeroCuenta = empresa.NumeroCuenta,
                    Cci = empresa.Cci,
                    YapeNumero = empresa.YapeNumero,
                    PlinNumero = empresa.PlinNumero,
                    QrPagoUrl = qrUrl,
                    InstruccionesPago = empresa.InstruccionesPago,
                    UsuarioModificacion = "admin"
                });

                return Ok(ResponseApiService.Response(StatusCodes.Status200OK, new { qrPagoUrl = qrUrl }, "QR actualizado"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }

        [HttpGet("facturacion-config")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFacturacionConfig(
            [FromServices] IGetAllEmpresaQuery getAllEmpresaQuery,
            [FromServices] IConfiguration configuration)
        {
            var integrado = configuration.GetValue<bool>("Sunat:Integrado");
            var data = await getAllEmpresaQuery.ExecuteGetFacturacionConfig(integrado);

            if (data == null)
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "No hay empresa activa"));

            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpPost("upload-certificado/{id}")]
        [RequestSizeLimit(2 * 1024 * 1024)]
        public async Task<IActionResult> UploadCertificado(
            int id,
            [FromServices] IFileStorageService fileStorage,
            [FromServices] IUpdateEmpresaCommand updateEmpresaCommand,
            [FromServices] IGetAllEmpresaQuery getAllEmpresaQuery,
            IFormFile archivo,
            [FromForm] string claveCertificado)
        {
            var empresa = await getAllEmpresaQuery.ExecuteGetEmpresaById(id);
            if (empresa == null)
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Empresa no encontrada"));

            try
            {
                var fileName = await fileStorage.SaveCertificadoAsync(archivo, claveCertificado);
                var updated = await updateEmpresaCommand.Execute(new UpdateEmpresaModel
                {
                    EmpresaID = empresa.EmpresaID,
                    RazonSocial = empresa.RazonSocial,
                    NombreComercial = empresa.NombreComercial,
                    RUC = empresa.RUC,
                    Email = empresa.Email,
                    Telefono = empresa.Telefono,
                    TelefonoSecundario = empresa.TelefonoSecundario,
                    WhatsApp = empresa.WhatsApp,
                    Direccion = empresa.Direccion,
                    Ciudad = empresa.Ciudad,
                    Pais = empresa.Pais,
                    Facebook = empresa.Facebook,
                    Instagram = empresa.Instagram,
                    LinkedIn = empresa.LinkedIn,
                    Twitter = empresa.Twitter,
                    HorarioAtencion = empresa.HorarioAtencion,
                    Logo = empresa.Logo,
                    BancoNombre = empresa.BancoNombre,
                    NumeroCuenta = empresa.NumeroCuenta,
                    Cci = empresa.Cci,
                    YapeNumero = empresa.YapeNumero,
                    PlinNumero = empresa.PlinNumero,
                    QrPagoUrl = empresa.QrPagoUrl,
                    InstruccionesPago = empresa.InstruccionesPago,
                    MontoAdelantoReserva = empresa.MontoAdelantoReserva,
                    GeneraFactElect = empresa.GeneraFactElect,
                    Ubigeo = empresa.Ubigeo,
                    RutaCertificadoServidor = empresa.RutaCertificadoServidor ?? "fe/certificado",
                    CertificadoFileName = fileName,
                    ClaveCertificado = claveCertificado,
                    UsuarioSol = empresa.UsuarioSol,
                    ClaveSol = null,
                    SunatModo = empresa.SunatModo,
                    SunatWsProduccion = empresa.SunatWsProduccion,
                    UsuarioModificacion = "admin"
                });

                if (!updated)
                    return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Empresa no encontrada"));

                return Ok(ResponseApiService.Response(StatusCodes.Status200OK, new { certificadoFileName = fileName }, "Certificado actualizado"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }
    }
}
