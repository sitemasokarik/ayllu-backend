using FluentValidation;
using DcodePe.Catering.Application.DataBase.Producto.Commands.Create;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.Validators.Producto
{
    public class CreateProductoValidator : AbstractValidator<CreateProductoModel>
    {
        private readonly IDataBaseService _databaseService;

        public CreateProductoValidator(IDataBaseService databaseService)
        {
            _databaseService = databaseService;

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del producto es obligatorio")
                .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres");

            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");

            RuleFor(x => x.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0");

            RuleFor(x => x.PrecioCosto)
                .GreaterThanOrEqualTo(0).WithMessage("El precio de costo debe ser mayor o igual a 0");

            RuleFor(x => x.CategoriaID)
                .GreaterThan(0).WithMessage("Debe seleccionar una categoría válida")
                .MustAsync(async (categoriaId, cancellation) => await CategoriaExists(categoriaId))
                .WithMessage("La categoría seleccionada no existe o está inactiva");

            //RuleFor(x => x.ImagenUrl)
            //    .MaximumLength(500).WithMessage("La URL de la imagen no puede exceder 500 caracteres");

            RuleFor(x => x.UsuarioCreacion)
                .NotEmpty().WithMessage("El usuario de creación es obligatorio")
                .MaximumLength(100).WithMessage("El usuario de creación no puede exceder 100 caracteres");
        }

        private async Task<bool> CategoriaExists(int categoriaId)
        {
            return await _databaseService.Categoria
                .AnyAsync(c => c.CategoriaID == categoriaId && c.Estado == true);
        }
    }
}
