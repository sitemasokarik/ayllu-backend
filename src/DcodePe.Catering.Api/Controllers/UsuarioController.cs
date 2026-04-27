using DcodePe.Catering.Application.DataBase.Usuario.Queries.GetAllUsuario;
using DcodePe.Catering.Application.DataBase.Usuario.Queries.GetUsuarioByCredentials;
using DcodePe.Catering.Application.DataBase.Usuario.Queries.GetUsuarioById;
using DcodePe.Catering.Application.DataBase.Usuario.Commands.Create;
using DcodePe.Catering.Application.DataBase.Usuario.Commands.Update;
using DcodePe.Catering.Application.DataBase.Usuario.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Usuario.Commands.UpdatePassword;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [Route("api/v1/usuario")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class UsuarioController : ControllerBase
    {
        /// <summary>
        /// Obtiene todos los usuarios activos
        /// </summary>
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromServices] IGetAllUsuarioQuery getAllUsuarioQuery)
        {
            var data = await getAllUsuarioQuery.ExecuteListUsuario();
            return StatusCode(StatusCodes.Status200OK,
             ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            [FromServices] IGetUsuarioByIdQuery getUsuarioByIdQuery,
            int id)
        {
            var data = await getUsuarioByIdQuery.Execute(id);

            if (data == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Usuario no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        /// <summary>
        /// Crea un nuevo usuario con contraseña hasheada con BCrypt
        /// </summary>
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create(
            [FromServices] ICreateUsuarioCommand createUsuarioCommand,
            [FromServices] IValidator<CreateUsuarioModel> validator,
            [FromBody] CreateUsuarioModel model)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            var data = await createUsuarioCommand.Execute(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Usuario creado exitosamente"));
        }

        /// <summary>
        /// Actualiza un usuario existente (NO actualiza password)
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateUsuarioCommand updateUsuarioCommand,
            [FromServices] IValidator<UpdateUsuarioModel> validator,
            [FromBody] UpdateUsuarioModel model)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            var result = await updateUsuarioCommand.Execute(model);

            if (!result)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Usuario no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, true, "Usuario actualizado exitosamente"));
        }

        /// <summary>
        /// Elimina un usuario (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteUsuarioCommand deleteUsuarioCommand,
            int id)
        {
            var result = await deleteUsuarioCommand.Execute(id, User.Identity?.Name ?? "SYSTEM");

            if (!result)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Usuario no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, true, "Usuario eliminado exitosamente"));
        }

        /// <summary>
        /// Endpoint de autenticación para Usuario. Permite login con UserName o Email.
        /// Las contraseñas son validadas con BCrypt.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromServices] IGetUsuarioByCredentialsQuery getUsuarioByCredentialsQuery,
            [FromServices] IGetTokenJwtService getTokenJwtService,
            [FromServices] IValidator<(string, string)> validator,
            [FromBody] LoginUsuarioRequest request)
        {
            // Validar credenciales
            var validationResult = await validator.ValidateAsync((request.UserName, request.Password));

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            // Buscar usuario por credenciales (BCrypt verification)
            var usuario = await getUsuarioByCredentialsQuery.Execute(request.UserName, request.Password);

            if (usuario == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Usuario o contraseña incorrectos"));

            // Generar token JWT
            usuario.Token = getTokenJwtService.Execute(usuario.UsuarioID.ToString());

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, usuario, "Login exitoso"));
        }

        /// <summary>
        /// Cambia la contraseña del usuario. Valida la contraseña actual y hashea la nueva con BCrypt.
        /// </summary>
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(
            [FromServices] IUpdateUsuarioPasswordCommand updateUsuarioPasswordCommand,
            [FromServices] IValidator<UpdateUsuarioPasswordModel> validator,
            [FromBody] UpdateUsuarioPasswordModel model)
        {
            // Validar modelo
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            // Ejecutar cambio de contraseña
            var result = await updateUsuarioPasswordCommand.Execute(model);

            if (!result)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, null, "No se pudo cambiar la contraseña. Verifica tu contraseña actual."));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, true, "Contraseña actualizada exitosamente"));
        }
    }

    /// <summary>
    /// Modelo de request para login
    /// </summary>
    public class LoginUsuarioRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
