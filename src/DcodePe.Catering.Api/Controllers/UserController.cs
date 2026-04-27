#if DEBUG // ⚠️ Solo disponible en DESARROLLO - Usar UsuarioController en producción
namespace DcodePe.Catering.Api.Controllers
{
#pragma warning disable
    [Authorize]
    [Route("api/v1/user")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApiAll)]
    [TypeFilter(typeof(ExceptionManager))]
    public class UserController : ControllerBase
    {
        private readonly IInsertApplicationInsightsService _insertApplicationInsightsService;
        public UserController(IInsertApplicationInsightsService insertApplicationInsightsService)
        {
            _insertApplicationInsightsService = insertApplicationInsightsService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(

            [FromBody] CreateUserModel model,
            [FromServices] ICreateUserCommand createUserCommand,
            [FromServices] IValidator<CreateUserModel> validator)
        {
            var validate = await validator.ValidateAsync(model);

            if (!validate.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validate.Errors, "Error de validacion"));

            var data = await createUserCommand.Execute(model);

            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Insertado con exito"));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(
            [FromBody] UpdateUserModel model,
            [FromServices] IUpdateUserCommand updateUserCommand,
            [FromServices] IValidator<UpdateUserModel> validator)
        {
            var update = await validator.ValidateAsync(model);

            if (!update.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, update.Errors, "Error de validacion"));
            var data = await updateUserCommand.Execute(model);

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Actualizado con exito"));
        }

        [HttpPut("updatepassword")]
        public async Task<IActionResult> UpdatePassword(
            [FromBody] UpdateUserPasswordModel model,
            [FromServices] IUpdateUserPasswordCommand updateuserpasswordCommand,
            [FromServices] IValidator<UpdateUserPasswordModel> validator)
        {
            var update = await validator.ValidateAsync(model);

            if (!update.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, update.Errors, "Error de validacion"));

            var data = await updateuserpasswordCommand.Execute(model);

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Contraseña actualizada con exito"));


        }

        [HttpDelete("delete/{userId}")]

        public async Task<IActionResult> Delete(
            int userId,
            [FromServices] IDeleteUserCommand deleteUserCommand)
        {

            if (userId == 0)
             return StatusCode(StatusCodes.Status400BadRequest,
                ResponseApiService.Response(StatusCodes.Status400BadRequest,  "No se elimino"));

            var data = await deleteUserCommand.Execute(userId);

            if (!data)
                return StatusCode(StatusCodes.Status404NotFound,
                ResponseApiService.Response(StatusCodes.Status404NotFound, data, "No se encontro"));

            return StatusCode(StatusCodes.Status200OK,
               ResponseApiService.Response(StatusCodes.Status200OK, data, "Eliminado con exito"));
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll( [FromServices] IGetAllUserQuery getAllUserQuery)
        {

            var metric = new InsertApplicationInsightsModel(
               ApplicationInsightsConstants.METRIC_TYPE_API_CALL,
               EntitiesConstants.USER,
               "getall");

            _insertApplicationInsightsService.Execute(metric);
            var data = await getAllUserQuery.Execute();
            if(data ==null || data.Count == 0)
                return StatusCode(StatusCodes.Status404NotFound,
                ResponseApiService.Response(StatusCodes.Status404NotFound, data, "No se encontro"));

            return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));

        }

        [HttpGet("getbyid/{userId}")]
        public async Task<IActionResult> GetById(
        int userId,
        [FromServices] IGetUserByIdQuery getUserByIdQuery)
        {
            var metric = new InsertApplicationInsightsModel(
               ApplicationInsightsConstants.METRIC_TYPE_API_CALL,
               EntitiesConstants.USER,
               "getbyid");

            _insertApplicationInsightsService.Execute(metric);

            if (userId == 0)
                return StatusCode(StatusCodes.Status400BadRequest,
                ResponseApiService.Response(StatusCodes.Status400BadRequest, "No se encontro"));

            var data = await getUserByIdQuery.Execute(userId);
            if (data == null)
                return StatusCode(StatusCodes.Status404NotFound,
                ResponseApiService.Response(StatusCodes.Status404NotFound, data, "No se encontro"));

            return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));

        }


        [AllowAnonymous]
        [HttpGet("getbyusernamepassword/{userName}/{password}")]
        public async Task<IActionResult> GetByUserNamePassword(
            string userName,
            string password,
            [FromServices] IGetUserByUserNameAndPasswordQuery getUserByUserNamePasswordQuery,
            [FromServices] IValidator<(string, string)> validator,
            [FromServices] IGetTokenJwtService getTokenJwtService)
        {

            var metric = new InsertApplicationInsightsModel(
                ApplicationInsightsConstants.METRIC_TYPE_API_CALL,
                EntitiesConstants.USER,
                "getbyusernamepassword");

            _insertApplicationInsightsService.Execute(metric);


            var validate = await validator.ValidateAsync((userName, password));

            if (!validate.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validate.Errors, "Error de validacion"));

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return StatusCode(StatusCodes.Status400BadRequest,
                ResponseApiService.Response(StatusCodes.Status400BadRequest, "No se encontro"));

            var data = await getUserByUserNamePasswordQuery.Execute(userName, password);

            if (data == null)
                return StatusCode(StatusCodes.Status404NotFound,
                ResponseApiService.Response(StatusCodes.Status404NotFound, data, "No se encontro"));
             data.Token = getTokenJwtService.Execute(data.UserId.ToString());
            return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));

        }
    }
}
#endif
