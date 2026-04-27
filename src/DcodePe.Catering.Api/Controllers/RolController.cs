using DcodePe.Catering.Application.DataBase.Rol.Commands.Create;
using DcodePe.Catering.Application.DataBase.Rol.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Rol.Commands.Update;
using DcodePe.Catering.Application.DataBase.Rol.Queries.GetAll;
using DcodePe.Catering.Application.DataBase.Rol.Queries.GetById;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [Route("api/v1/rol")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class RolController : ControllerBase
    {
        /// <summary>
        /// Obtiene todos los roles activos
        /// </summary>
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllRolQuery getAllRolQuery)
        {
            var data = await getAllRolQuery.Execute();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        /// <summary>
        /// Obtiene un rol por su ID
        /// </summary>
        [HttpGet("getbyid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromServices] IGetRolByIdQuery getRolByIdQuery,
            int id)
        {
            var data = await getRolByIdQuery.Execute(id);

            if (data == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Rol no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        /// <summary>
        /// Crea un nuevo rol
        /// </summary>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromServices] ICreateRolCommand createRolCommand,
            [FromServices] IValidator<CreateRolModel> validator,
            [FromBody] CreateRolModel model)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            try
            {
                var data = await createRolCommand.Execute(model);
                return StatusCode(StatusCodes.Status201Created,
                    ResponseApiService.Response(StatusCodes.Status201Created, data, "Rol creado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }

        /// <summary>
        /// Actualiza un rol existente
        /// </summary>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateRolCommand updateRolCommand,
            [FromServices] IValidator<UpdateRolModel> validator,
            [FromBody] UpdateRolModel model)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            try
            {
                var result = await updateRolCommand.Execute(model);

                if (!result)
                    return StatusCode(StatusCodes.Status404NotFound,
                        ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Rol no encontrado"));

                return StatusCode(StatusCodes.Status200OK,
                    ResponseApiService.Response(StatusCodes.Status200OK, true, "Rol actualizado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }

        /// <summary>
        /// Elimina un rol (soft delete)
        /// Solo si no tiene usuarios asignados
        /// </summary>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteRolCommand deleteRolCommand,
            int id)
        {
            try
            {
                var result = await deleteRolCommand.Execute(id, User.Identity?.Name ?? "SYSTEM");

                if (!result)
                    return StatusCode(StatusCodes.Status404NotFound,
                        ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Rol no encontrado"));

                return StatusCode(StatusCodes.Status200OK,
                    ResponseApiService.Response(StatusCodes.Status200OK, true, "Rol eliminado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }
    }
}
