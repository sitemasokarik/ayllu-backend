//#if DEBUG //  Solo disponible en DESARROLLO
using DcodePe.Catering.Application.DataBase.Categoria.Commands.Create;
using DcodePe.Catering.Application.DataBase.Categoria.Commands.Update;
using DcodePe.Catering.Application.DataBase.Categoria.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Categoria.Queries.GetAll;
using DcodePe.Catering.Application.DataBase.Categoria.Queries.GetById;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [Route("api/v1/categoria")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class CategoriaController : ControllerBase
    {
        /// <summary>
        /// Obtiene todas las categorías activas (lista plana sin jerarquía)
        /// </summary>
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllCategoriaQuery getAllCategoriaQuery)
        {
            var data = await getAllCategoriaQuery.Execute();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        /// <summary>
        /// Obtiene toda la jerarquía de categorías desde la raíz (estructura recursiva)
        /// </summary>
        [HttpGet("hierarchy")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHierarchy(
            [FromServices] IGetAllCategoriaQuery getAllCategoriaQuery)
        {
            var jerarquia = await getAllCategoriaQuery.ExecuteHierarchy();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, jerarquia, "Jerarquía obtenida exitosamente"));
        }

        /// <summary>
        /// Obtiene solo las categorías raíz (nivel 0) con sus hijos directos
        /// </summary>
        [HttpGet("root")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRootCategories(
            [FromServices] IGetAllCategoriaQuery getAllCategoriaQuery)
        {
            var categorias = await getAllCategoriaQuery.ExecuteGetRootCategories();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, categorias, "Categorías raíz obtenidas"));
        }

        /// <summary>
        /// Obtiene una categoría por su ID, incluyendo información del padre (CategoriaPadreID y CategoriaPadreNombre)
        /// </summary>
        /// <param name="id">ID de la categoría a buscar</param>
        /// <returns>Categoría con información completa incluyendo referencia al padre</returns>
        [HttpGet("getbyid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromServices] IGetCategoriaByIdQuery getCategoriaByIdQuery,
            int id)
        {
            var data = await getCategoriaByIdQuery.Execute(id);

            if (data == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Categoría no encontrada"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        /// <summary>
        /// Obtiene una categoría con todos sus hijos recursivamente
        /// </summary>
        [HttpGet("{id}/children")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWithChildren(
            [FromServices] IGetAllCategoriaQuery getAllCategoriaQuery,
            int id)
        {
            var categoria = await getAllCategoriaQuery.ExecuteGetByIdWithChildren(id);
            
            if (categoria == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Categoría no encontrada"));
            
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, categoria, "Categoría con hijos obtenida"));
        }

        /// <summary>
        /// Obtiene el path completo de una categoría (breadcrumb)
        /// Ejemplo: Alimentos > Entradas > Entradas Frías
        /// </summary>
        [HttpGet("{id}/path")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPath(
            [FromServices] IGetAllCategoriaQuery getAllCategoriaQuery,
            int id)
        {
            var path = await getAllCategoriaQuery.ExecuteGetCategoryPath(id);
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, path, "Path de categoría obtenido"));
        }

        /// <summary>
        /// Obtiene todos los IDs de las categorías descendientes (hijos, nietos, etc.)
        /// </summary>
        [HttpGet("{id}/descendants")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDescendants(
            [FromServices] IGetAllCategoriaQuery getAllCategoriaQuery,
            int id)
        {
            var descendientes = await getAllCategoriaQuery.ExecuteGetAllDescendants(id);
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, descendientes, "Descendientes obtenidos"));
        }

        /// <summary>
        /// Crea una nueva categoría
        /// </summary>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromServices] ICreateCategoriaCommand createCategoriaCommand,
            [FromServices] IValidator<CreateCategoriaModel> validator,
            [FromBody] CreateCategoriaModel model)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            var data = await createCategoriaCommand.Execute(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Categoría creada exitosamente"));
        }

        /// <summary>
        /// Actualiza una categoría existente
        /// Valida que no se creen referencias circulares en la jerarquía
        /// </summary>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateCategoriaCommand updateCategoriaCommand,
            [FromServices] IValidator<UpdateCategoriaModel> validator,
            [FromBody] UpdateCategoriaModel model)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            try
            {
                var result = await updateCategoriaCommand.Execute(model);

                if (!result)
                    return StatusCode(StatusCodes.Status404NotFound,
                        ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Categoría no encontrada"));

                return StatusCode(StatusCodes.Status200OK,
                    ResponseApiService.Response(StatusCodes.Status200OK, true, "Categoría actualizada exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }

        /// <summary>
        /// Elimina una categoría (soft delete)
        /// Solo si no tiene productos ni subcategorías asociadas
        /// </summary>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteCategoriaCommand deleteCategoriaCommand,
            int id)
        {
            try
            {
                var result = await deleteCategoriaCommand.Execute(id, User.Identity?.Name ?? "SYSTEM");

                if (!result)
                    return StatusCode(StatusCodes.Status404NotFound,
                        ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Categoría no encontrada"));

                return StatusCode(StatusCodes.Status200OK,
                    ResponseApiService.Response(StatusCodes.Status200OK, true, "Categoría eliminada exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }
    }
}
//#endif
