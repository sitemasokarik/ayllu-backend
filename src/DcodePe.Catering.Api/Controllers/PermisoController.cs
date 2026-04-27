using DcodePe.Catering.Application.DataBase.Permiso.Commands.Create;
using DcodePe.Catering.Application.DataBase.Permiso.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Permiso.Commands.Update;
using DcodePe.Catering.Application.DataBase.Permiso.Queries.GetAll;
using DcodePe.Catering.Application.DataBase.Permiso.Queries.GetByRolId;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [Route("api/v1/permiso")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class PermisoController : ControllerBase
    {
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllPermisoQuery getAllPermisoQuery)
        {
            var data = await getAllPermisoQuery.Execute();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("getbyrolid/{rolId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByRolId(
            [FromServices] IGetPermisosByRolIdQuery getPermisosByRolIdQuery,
            int rolId)
        {
            var data = await getPermisosByRolIdQuery.Execute(rolId);
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromServices] ICreatePermisoCommand createPermisoCommand,
            [FromBody] CreatePermisoModel model)
        {
            try
            {
                var data = await createPermisoCommand.Execute(model);
                return StatusCode(StatusCodes.Status201Created,
                    ResponseApiService.Response(StatusCodes.Status201Created, data, "Permiso creado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdatePermisoCommand updatePermisoCommand,
            [FromBody] UpdatePermisoModel model)
        {
            try
            {
                var result = await updatePermisoCommand.Execute(model);

                if (!result)
                    return StatusCode(StatusCodes.Status404NotFound,
                        ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Permiso no encontrado"));

                return StatusCode(StatusCodes.Status200OK,
                    ResponseApiService.Response(StatusCodes.Status200OK, true, "Permiso actualizado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromServices] IDeletePermisoCommand deletePermisoCommand,
            int id)
        {
            try
            {
                var result = await deletePermisoCommand.Execute(id, User.Identity?.Name ?? "SYSTEM");

                if (!result)
                    return StatusCode(StatusCodes.Status404NotFound,
                        ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Permiso no encontrado"));

                return StatusCode(StatusCodes.Status200OK,
                    ResponseApiService.Response(StatusCodes.Status200OK, true, "Permiso eliminado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }
    }
}
