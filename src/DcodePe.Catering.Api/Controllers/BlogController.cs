using DcodePe.Catering.Application.DataBase.Blog.Commands.Create;
using DcodePe.Catering.Application.DataBase.Blog.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Blog.Commands.Update;
using DcodePe.Catering.Application.DataBase.Blog.Queries.GetAllBlog;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [Route("api/v1/blog")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class BlogController : ControllerBase
    {
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateBlogModel model,
            [FromServices] ICreateBlogCommand createBlogCommand)
        {
            var data = await createBlogCommand.Execute(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Landing Page creada exitosamente"));
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateBlogCommand updateBlogCommand,
            [FromBody] UpdateBlogModel model)
        {
            var result = await updateBlogCommand.Execute(model);

            if (!result)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Landing Page no encontrada"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, result, "Landing Page actualizada exitosamente"));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteBlogCommand deleteBlogCommand,
            int id,
            [FromQuery] string usuarioEliminacion = "SYSTEM")
        {
            var result = await deleteBlogCommand.Execute(id, usuarioEliminacion);

            if (!result)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Landing Page no encontrada"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, result, "Landing Page eliminada exitosamente"));
        }

        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllBlogQuery getAllBlogQuery)
        {
            var data = await getAllBlogQuery.ExecuteListBlog();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        [HttpGet("getbyid/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            int id,
            [FromServices] IGetAllBlogQuery getAllBlogQuery)
        {
            var data = await getAllBlogQuery.ExecuteGetBlogById(id);

            if (data == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Landing Page no encontrada"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }
    }
}

