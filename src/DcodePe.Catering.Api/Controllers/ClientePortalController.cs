using DcodePe.Catering.Application.Database;
using DcodePe.Catering.Api.Helpers;
using DcodePe.Catering.Application.DataBase.Cliente.Commands.ChangePortalPassword;
using DcodePe.Catering.Application.DataBase.Cliente.Commands.UpdatePortalProfile;
using DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetAllCotizacion;
using DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetByClientePortal;
using DcodePe.Catering.Application.DataBase.Empresa.Queries.GetAllEmpresa;
using DcodePe.Catering.Application.DataBase.MercadoPago.Commands.CreatePreference;
using DcodePe.Catering.Application.DataBase.MercadoPago.Queries.GetPagoEstado;
using DcodePe.Catering.Application.DataBase.PagoVoucher.Commands.UploadPortal;
using DcodePe.Catering.Application.DataBase.Ticket.Commands.AddMensaje;
using DcodePe.Catering.Application.DataBase.Ticket.Commands.Create;
using DcodePe.Catering.Application.DataBase.Ticket.Queries.GetAllTicket;
using DcodePe.Catering.Application.DataBase.Ticket.Queries.GetTicketDetalle;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/Cliente/portal")]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class ClientePortalController : ControllerBase
    {
        private IActionResult RequireCliente()
        {
            if (!PortalAuthHelper.IsCliente(User))
                return Forbid();
            return null!;
        }

        [HttpGet("mis-cotizaciones")]
        public async Task<IActionResult> MisCotizaciones(
            [FromServices] IGetCotizacionesByClientePortalQuery query)
        {
            var denied = RequireCliente();
            if (denied != null) return denied;

            var clienteId = PortalAuthHelper.GetClienteId(User)!.Value;
            var data = await query.Execute(clienteId);
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("mis-cotizaciones/{cotizacionId}")]
        public async Task<IActionResult> CotizacionDetalle(
            int cotizacionId,
            [FromServices] IGetAllCotizacionQuery cotizacionQuery)
        {
            var denied = RequireCliente();
            if (denied != null) return denied;

            var clienteId = PortalAuthHelper.GetClienteId(User)!.Value;
            var items = await cotizacionQuery.ExecuteListaCotizacionById(cotizacionId);
            var cotizacion = items.FirstOrDefault();

            if (cotizacion == null || cotizacion.ClienteID != clienteId)
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Cotización no encontrada"));

            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, cotizacion, "Consulta exitosa"));
        }

        [HttpGet("medios-pago")]
        [AllowAnonymous]
        public async Task<IActionResult> MediosPago([FromServices] IGetAllEmpresaQuery empresaQuery)
        {
            var empresa = await empresaQuery.ExecuteGetEmpresaActiva();
            if (empresa == null)
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "No hay empresa activa"));

            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, new
            {
                empresa.BancoNombre,
                empresa.NumeroCuenta,
                empresa.Cci,
                empresa.YapeNumero,
                empresa.PlinNumero,
                empresa.QrPagoUrl,
                empresa.InstruccionesPago,
                empresa.RazonSocial,
                empresa.RUC,
                montoAdelantoReserva = empresa.MontoAdelantoReserva > 0 ? empresa.MontoAdelantoReserva : 1000m,
                cuentasPago = empresa.CuentasPago
            }, "Consulta exitosa"));
        }

        [HttpPost("cotizaciones/{cotizacionId}/mercadopago/preference")]
        public async Task<IActionResult> CrearPreferenceMercadoPago(
            int cotizacionId,
            [FromServices] ICreateMercadoPagoPreferenceCommand command,
            [FromBody] ReservarAdelantoRequest? request)
        {
            var denied = RequireCliente();
            if (denied != null) return denied;

            try
            {
                var clienteId = PortalAuthHelper.GetClienteId(User)!.Value;
                var data = await command.Execute(cotizacionId, clienteId, request?.FechaReservadaElegida);
                var publicKey = HttpContext.RequestServices
                    .GetRequiredService<IConfiguration>()["MercadoPago:PublicKey"];

                return Ok(ResponseApiService.Response(StatusCodes.Status200OK, new
                {
                    data.PreferenceId,
                    data.InitPoint,
                    data.Monto,
                    publicKey
                }, "Preferencia creada"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }

        [HttpGet("cotizaciones/{cotizacionId}/pago-estado")]
        public async Task<IActionResult> PagoEstado(
            int cotizacionId,
            [FromServices] IGetCotizacionPagoEstadoQuery query)
        {
            var denied = RequireCliente();
            if (denied != null) return denied;

            var clienteId = PortalAuthHelper.GetClienteId(User)!.Value;
            var data = await query.Execute(cotizacionId, clienteId);

            if (data == null)
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Cotización no encontrada"));

            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpPost("cotizaciones/{cotizacionId}/voucher")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> SubirVoucher(
            int cotizacionId,
            [FromServices] IUploadPagoVoucherPortalCommand command,
            [FromServices] IFileStorageService fileStorage,
            [FromServices] IDataBaseService db,
            [FromForm] SubirVoucherPortalRequest request)
        {
            var denied = RequireCliente();
            if (denied != null) return denied;

            try
            {
                var clienteId = PortalAuthHelper.GetClienteId(User)!.Value;
                var cliente = await db.Cliente.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.ClienteID == clienteId);
                if (cliente == null)
                    return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Cliente no encontrado"));

                var archivoUrl = await fileStorage.SaveVoucherAsync(request.Archivo);
                var monto = request.Monto > 0 ? request.Monto : 0;

                await command.Execute(new UploadPagoVoucherPortalModel
                {
                    CotizacionID = cotizacionId,
                    ClienteID = clienteId,
                    ArchivoUrl = archivoUrl,
                    NombreArchivo = request.Archivo.FileName,
                    Monto = monto,
                    FechaReservadaElegida = request.FechaReservadaElegida,
                    ObservacionCliente = request.Observacion,
                    UsuarioCreacion = cliente.Email ?? "portal"
                });

                return Ok(ResponseApiService.Response(StatusCodes.Status200OK, true,
                    "Voucher enviado. Tu cotización quedó pendiente de revisión."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }

        [HttpGet("perfil")]
        public async Task<IActionResult> Perfil([FromServices] IDataBaseService db)
        {
            var denied = RequireCliente();
            if (denied != null) return denied;

            var clienteId = PortalAuthHelper.GetClienteId(User)!.Value;
            var cliente = await db.Cliente
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ClienteID == clienteId && c.Estado == true);

            if (cliente == null)
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Cliente no encontrado"));

            var partes = SplitNombre(cliente.NombreCompleto);
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, new
            {
                cliente.ClienteID,
                cliente.NumeroDocumento,
                cliente.TipoDocumento,
                Nombres = partes.nombres,
                Apellidos = partes.apellidos,
                cliente.NombreCompleto,
                cliente.Email,
                cliente.Telefono,
                TotalCotizaciones = await db.Cotizacion.CountAsync(c => c.ClienteID == clienteId && c.Estado == true)
            }, "Consulta exitosa"));
        }

        [HttpPut("perfil")]
        public async Task<IActionResult> ActualizarPerfil(
            [FromServices] IUpdateClientePortalProfileCommand command,
            [FromBody] UpdateClientePortalProfileModel model)
        {
            var denied = RequireCliente();
            if (denied != null) return denied;

            try
            {
                var clienteId = PortalAuthHelper.GetClienteId(User)!.Value;
                var data = await command.Execute(clienteId, model);
                return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Perfil actualizado"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }

        [HttpPut("cambiar-contrasena")]
        public async Task<IActionResult> CambiarContrasena(
            [FromServices] IChangeClientePortalPasswordCommand command,
            [FromBody] ChangeClientePortalPasswordModel model)
        {
            var denied = RequireCliente();
            if (denied != null) return denied;

            try
            {
                var clienteId = PortalAuthHelper.GetClienteId(User)!.Value;
                await command.Execute(clienteId, model);
                return Ok(ResponseApiService.Response(StatusCodes.Status200OK, true, "Contraseña actualizada"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }

        [HttpGet("tickets")]
        public async Task<IActionResult> MisTickets([FromServices] IGetAllTicketQuery query)
        {
            var denied = RequireCliente();
            if (denied != null) return denied;

            var clienteId = PortalAuthHelper.GetClienteId(User)!.Value;
            var data = await query.Execute(clienteId: clienteId);
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("tickets/{ticketId}")]
        public async Task<IActionResult> TicketDetalle(
            [FromServices] IGetTicketDetalleQuery query,
            int ticketId)
        {
            var denied = RequireCliente();
            if (denied != null) return denied;

            var clienteId = PortalAuthHelper.GetClienteId(User)!.Value;
            var data = await query.Execute(ticketId, clienteId);
            if (data == null)
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Ticket no encontrado"));

            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpPost("tickets")]
        public async Task<IActionResult> CrearTicket(
            [FromServices] ICreateTicketCommand createCommand,
            [FromServices] IAddTicketMensajeCommand mensajeCommand,
            [FromServices] IDataBaseService db,
            [FromBody] CrearTicketPortalRequest request)
        {
            var denied = RequireCliente();
            if (denied != null) return denied;

            var clienteId = PortalAuthHelper.GetClienteId(User)!.Value;
            var cliente = await db.Cliente.AsNoTracking().FirstOrDefaultAsync(c => c.ClienteID == clienteId);
            if (cliente == null)
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Cliente no encontrado"));

            var ticket = await createCommand.Execute(new CreateTicketModel
            {
                Titulo = request.Titulo,
                Descripcion = request.Descripcion,
                Prioridad = string.IsNullOrWhiteSpace(request.Prioridad) ? "Media" : request.Prioridad,
                EstadoTicket = "Abierto",
                CreadoPorClienteID = clienteId,
                RolDestinoID = request.Destino == "soporte" ? 1 : 2,
                CotizacionID = request.CotizacionID,
                UsuarioCreacion = cliente.Email ?? "portal"
            });

            await mensajeCommand.Execute(new AddTicketMensajeModel
            {
                TicketID = ticket.TicketID,
                ClienteID = clienteId,
                AutorNombre = cliente.NombreCompleto,
                Mensaje = request.Descripcion,
                EsInterno = false,
                UsuarioCreacion = cliente.Email ?? "portal"
            });

            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, ticket, "Ticket creado"));
        }

        [HttpPost("tickets/mensaje")]
        public async Task<IActionResult> AgregarMensaje(
            [FromServices] IAddTicketMensajeCommand command,
            [FromServices] IGetTicketDetalleQuery detalleQuery,
            [FromServices] IDataBaseService db,
            [FromBody] AgregarMensajePortalRequest request)
        {
            var denied = RequireCliente();
            if (denied != null) return denied;

            var clienteId = PortalAuthHelper.GetClienteId(User)!.Value;
            var ticket = await detalleQuery.Execute(request.TicketID, clienteId);
            if (ticket == null)
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Ticket no encontrado"));

            var cliente = await db.Cliente.AsNoTracking().FirstOrDefaultAsync(c => c.ClienteID == clienteId);
            var data = await command.Execute(new AddTicketMensajeModel
            {
                TicketID = request.TicketID,
                ClienteID = clienteId,
                AutorNombre = cliente?.NombreCompleto ?? "Cliente",
                Mensaje = request.Mensaje,
                EsInterno = false,
                UsuarioCreacion = cliente?.Email ?? "portal"
            });

            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Mensaje enviado"));
        }

        private static (string nombres, string apellidos) SplitNombre(string nombreCompleto)
        {
            if (string.IsNullOrWhiteSpace(nombreCompleto))
                return (string.Empty, string.Empty);

            var parts = nombreCompleto.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length <= 1)
                return (parts[0], string.Empty);
            if (parts.Length == 2)
                return (parts[0], parts[1]);
            if (parts.Length == 3)
                return ($"{parts[0]} {parts[1]}", parts[2]);

            var mitad = parts.Length / 2;
            return (string.Join(' ', parts.Take(mitad)), string.Join(' ', parts.Skip(mitad)));
        }
    }

    public class CrearTicketPortalRequest
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Destino { get; set; } = "ventas";
        public string Prioridad { get; set; } = "Media";
        public int? CotizacionID { get; set; }
    }

    public class AgregarMensajePortalRequest
    {
        public int TicketID { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }

    public class SubirVoucherPortalRequest
    {
        public IFormFile Archivo { get; set; } = null!;
        public decimal Monto { get; set; }
        public DateTime? FechaReservadaElegida { get; set; }
        public string? Observacion { get; set; }
    }

    public class ReservarAdelantoRequest
    {
        public DateTime? FechaReservadaElegida { get; set; }
    }
}
