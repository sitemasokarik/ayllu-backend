//#if DEBUG // ⚠️ Solo disponible en DESARROLLO
using DcodePe.Catering.Application.DataBase.Cliente.Commands.Create;
using DcodePe.Catering.Application.DataBase.Cliente.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Cliente.Commands.Update;
using DcodePe.Catering.Application.DataBase.Cliente.Commands.RegisterPortal;
using DcodePe.Catering.Application.DataBase.Cliente.Queries.GetAllCliente;
using DcodePe.Catering.Application.DataBase.Cliente.Queries.LoginPortal;
using DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetByClientePortal;

namespace DcodePe.Catering.Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de clientes del sistema de catering
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class ClienteController : ControllerBase
    {
        /// <summary>
        /// Crea un nuevo cliente en el sistema
        /// </summary>
        /// <param name="createClienteCommand">Comando de creación</param>
        /// <param name="validator">Validador de datos</param>
        /// <param name="model">Datos del cliente a crear</param>
        /// <returns>Cliente creado</returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> Create(
            [FromServices] ICreateClienteCommand createClienteCommand,
            [FromServices] IValidator<CreateClienteModel> validator,
            [FromBody] CreateClienteModel model)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            var data = await createClienteCommand.Execute(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Cliente creado exitosamente"));
        }

        /// <summary>
        /// Actualiza los datos de un cliente existente
        /// </summary>
        /// <param name="updateClienteCommand">Comando de actualización</param>
        /// <param name="validator">Validador de datos</param>
        /// <param name="model">Datos del cliente a actualizar</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateClienteCommand updateClienteCommand,
            [FromServices] IValidator<UpdateClienteModel> validator,
            [FromBody] UpdateClienteModel model)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            var result = await updateClienteCommand.Execute(model);

            if (!result)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Cliente no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, result, "Cliente actualizado exitosamente"));
        }

        /// <summary>
        /// Elimina un cliente (soft delete)
        /// </summary>
        /// <param name="deleteClienteCommand">Comando de eliminación</param>
        /// <param name="clienteId">ID del cliente a eliminar</param>
        /// <param name="usuarioEliminacion">Usuario que realiza la eliminación</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("delete/{clienteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteClienteCommand deleteClienteCommand,
            int clienteId,
            [FromQuery] string usuarioEliminacion = "SYSTEM")
        {
            try
            {
                var result = await deleteClienteCommand.Execute(clienteId, usuarioEliminacion);

                if (!result)
                    return StatusCode(StatusCodes.Status404NotFound,
                        ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Cliente no encontrado"));

                return StatusCode(StatusCodes.Status200OK,
                    ResponseApiService.Response(StatusCodes.Status200OK, result, "Cliente eliminado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }

        /// <summary>
        /// Obtiene todos los clientes activos del sistema
        /// </summary>
        /// <param name="getAllClienteQuery">Query de consulta</param>
        /// <returns>Lista de clientes</returns>
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromServices] IGetAllClienteQuery getAllClienteQuery)
        {
            var data = await getAllClienteQuery.Execute();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        /// <summary>
        /// Busca un cliente por su número de documento
        /// </summary>
        /// <param name="getAllClienteQuery">Query de consulta</param>
        /// <param name="numeroDocumento">Número de documento del cliente</param>
        /// <returns>Cliente encontrado</returns>
        [HttpGet("getbydocumento/{numeroDocumento}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetByNumeroDocumento(
            [FromServices] IGetAllClienteQuery getAllClienteQuery,
            string numeroDocumento)
        {
            var data = await getAllClienteQuery.GetByNumeroDocumento(numeroDocumento);

            if (data == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Cliente no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        /// <summary>
        /// Obtiene clientes VIP activos
        /// </summary>
        /// <param name="getAllClienteQuery">Query de consulta</param>
        /// <returns>Lista de clientes VIP</returns>
        [HttpGet("getvip")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVIP([FromServices] IGetAllClienteQuery getAllClienteQuery)
        {
            var data = await getAllClienteQuery.Execute();
            var clientesVIP = data.Where(c => c.EsVIP).ToList();

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, clientesVIP, "Consulta exitosa"));
        }

        /// <summary>
        /// Obtiene clientes por tipo (Natural, Juridica)
        /// </summary>
        /// <param name="getAllClienteQuery">Query de consulta</param>
        /// <param name="tipoCliente">Tipo de cliente a filtrar</param>
        /// <returns>Lista de clientes filtrada por tipo</returns>
        [HttpGet("getbytipo/{tipoCliente}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByTipo(
            [FromServices] IGetAllClienteQuery getAllClienteQuery,
            string tipoCliente)
        {
            var data = await getAllClienteQuery.Execute();
            var clientesFiltrados = data.Where(c => 
                c.TipoCliente.Equals(tipoCliente, StringComparison.OrdinalIgnoreCase)).ToList();

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, clientesFiltrados, "Consulta exitosa"));
        }

        [HttpGet("cotizaciones/{clienteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCotizacionesByCliente(
            [FromServices] IGetCotizacionesByClientePortalQuery query,
            int clienteId)
        {
            var data = await query.Execute(clienteId);
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        /// <summary>
        /// Registro de cliente en el portal público (landing)
        /// </summary>
        [AllowAnonymous]
        [HttpPost("portal/register")]
        public async Task<IActionResult> RegisterPortal(
            [FromServices] IRegisterClientePortalCommand command,
            [FromBody] RegisterClientePortalModel model)
        {
            try
            {
                var data = await command.Execute(model);
                var mensaje = data.TotalCotizacionesVinculadas > 0
                    ? $"Cuenta creada. Se vincularon {data.TotalCotizacionesVinculadas} cotización(es) previas."
                    : "Cliente registrado en portal";
                return StatusCode(StatusCodes.Status201Created,
                    ResponseApiService.Response(StatusCodes.Status201Created, data, mensaje));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }

        /// <summary>
        /// Login de cliente en el portal público
        /// </summary>
        [AllowAnonymous]
        [HttpPost("portal/login")]
        public async Task<IActionResult> LoginPortal(
            [FromServices] ILoginClientePortalQuery query,
            [FromBody] LoginClientePortalRequest request)
        {
            var data = await query.Execute(request.Email, request.Password);
            if (data == null)
                return Unauthorized(ResponseApiService.Response(StatusCodes.Status401Unauthorized, null, "Credenciales inválidas"));
            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data, "Login exitoso"));
        }
    }

    public class LoginClientePortalRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
//#endif
