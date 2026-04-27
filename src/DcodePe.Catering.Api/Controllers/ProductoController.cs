//#if DEBUG // ?? Solo disponible en DESARROLLO
using DcodePe.Catering.Application.DataBase.Producto.Commands.Create;
using DcodePe.Catering.Application.DataBase.Producto.Commands.Update;
using DcodePe.Catering.Application.DataBase.Producto.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Producto.Queries.GetProducto;
using Microsoft.AspNetCore.Mvc;

namespace DcodePe.Catering.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(IgnoreApi = APICONST.ApiExplorer.IncludeApi)]
    [TypeFilter(typeof(ExceptionManager))]
    public class ProductoController : ControllerBase
    {
        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> Create(
            [FromServices] ICreateProductoCommand createProductoCommand,
            [FromServices] IValidator<CreateProductoModel> validator,
            [FromBody] CreateProductoModel model)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            var data = await createProductoCommand.ExecuteSaveProducto(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, data, "Producto creado exitosamente"));
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateProductoCommand updateProductoCommand,
            [FromServices] IValidator<UpdateProductoModel> validator,
            [FromBody] UpdateProductoModel model)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors, "Error de validación"));

            var result = await updateProductoCommand.Execute(model);

            if (!result)
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Producto no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, result, "Producto actualizado exitosamente"));
        }

        /// <summary>
        /// Elimina un producto (soft delete)
        /// </summary>
        [HttpDelete("delete/{productoId}")]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteProductoCommand deleteProductoCommand,
            int productoId,
            [FromQuery] string usuarioEliminacion = "SYSTEM")
        {
            try
            {
                var result = await deleteProductoCommand.Execute(productoId, usuarioEliminacion);

                if (!result)
                    return StatusCode(StatusCodes.Status404NotFound,
                        ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Producto no encontrado"));

                return StatusCode(StatusCodes.Status200OK,
                    ResponseApiService.Response(StatusCodes.Status200OK, result, "Producto eliminado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, null, ex.Message));
            }
        }

        /// <summary>
        /// Obtiene todos los productos activos
        /// </summary>
        [HttpGet("getall")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromServices] IGetProductoQuery getProductoQuery)
        {
            var data = await getProductoQuery.GetAllProductoAsync();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data, "Consulta exitosa"));
        }

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        [HttpGet("getbyid/{productoId}")]
        public async Task<IActionResult> GetById(
            [FromServices] IGetProductoQuery getProductoQuery, 
            int productoId)
        {
            var data = await getProductoQuery.GetProductoById(productoId);

            if (data == null || !data.Any())
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Producto no encontrado"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data.FirstOrDefault(), "Consulta exitosa"));
        }

        /// <summary>
        /// Obtiene un producto con sus paquetes asociados
        /// </summary>
        //[HttpGet("GetProductoWithPaquetesByProductoId/{productoId}")]
        //public async Task<IActionResult> GetProductoWithPaquetes(
        //    [FromServices] IGetProductoQuery getProductoQuery, 
        //    int productoId)
        //{
        //    var data = await getProductoQuery.ExecuteGetProductoWithPaquetesByProductoId(productoId);

        //    if (data == null || !data.Any())
        //        return StatusCode(StatusCodes.Status404NotFound,
        //            ResponseApiService.Response(StatusCodes.Status404NotFound, null, "Producto no encontrado"));

        //    return StatusCode(StatusCodes.Status200OK,
        //        ResponseApiService.Response(StatusCodes.Status200OK, data.FirstOrDefault(), "Consulta exitosa"));
        //}
    }
}

//#endif