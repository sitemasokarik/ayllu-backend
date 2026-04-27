using DcodePe.Catering.Application.DataBase.Pagina.Commands.Create;
using DcodePe.Catering.Application.DataBase.Pagina.Commands.Update;
using DcodePe.Catering.Application.DataBase.Pagina.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Pagina.Queries.GetAll;
using DcodePe.Catering.Application.DataBase.Pagina.Queries.GetById;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [Route("api/v1/pagina")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class PaginaController : ControllerBase
    {
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllPaginaQuery getAllPaginaQuery)
        {
            var data = await getAllPaginaQuery.Execute();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("getbyid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromServices] IGetPaginaByIdQuery getPaginaByIdQuery,
            int id)
        {
            var data = await getPaginaByIdQuery.Execute(id);

            if (data == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Página no encontrada"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromServices] ICreatePaginaCommand createPaginaCommand,
            [FromServices] IValidator<CreatePaginaModel> validator,
            [FromBody] CreatePaginaModel model)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            try
            {
                var data = await createPaginaCommand.Execute(model);
                return StatusCode(StatusCodes.Status201Created,
                    ResponseApiService.Response(StatusCodes.Status201Created, data, "Página creada exitosamente"));
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
            [FromServices] IUpdatePaginaCommand updatePaginaCommand,
            [FromServices] IValidator<UpdatePaginaModel> validator,
            [FromBody] UpdatePaginaModel model)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            try
            {
                var result = await updatePaginaCommand.Execute(model);

                if (!result)
                    return StatusCode(StatusCodes.Status404NotFound,
                        ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Página no encontrada"));

                return StatusCode(StatusCodes.Status200OK,
                    ResponseApiService.Response(StatusCodes.Status200OK, true, "Página actualizada exitosamente"));
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
            [FromServices] IDeletePaginaCommand deletePaginaCommand,
            int id)
        {
            try
            {
                var result = await deletePaginaCommand.Execute(id, User.Identity?.Name ?? "SYSTEM");

                if (!result)
                    return StatusCode(StatusCodes.Status404NotFound,
                        ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Página no encontrada"));

                return StatusCode(StatusCodes.Status200OK,
                    ResponseApiService.Response(StatusCodes.Status200OK, true, "Página eliminada exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }
    }
}
