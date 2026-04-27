using FluentValidation;
using DcodePe.Catering.Application.DataBase.Categoria.Commands.Update;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.Validators.Categoria
{
    public class UpdateCategoriaValidator : AbstractValidator<UpdateCategoriaModel>
    {
        private readonly IDataBaseService _databaseService;

        public UpdateCategoriaValidator(IDataBaseService databaseService)
        {
            _databaseService = databaseService;

            RuleFor(x => x.CategoriaID)
                .GreaterThan(0).WithMessage("El ID de la categoría es obligatorio");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre de la categoría es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres")
                .MustAsync(async (model, nombre, cancellation) => 
                    await BeUniqueNombre(model.CategoriaID, nombre))
                .WithMessage("Ya existe una categoría con ese nombre");

            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");

            RuleFor(x => x.Limite)
                .GreaterThanOrEqualTo(1).WithMessage("El límite debe ser mayor o igual a 1");

            RuleFor(x => x.UsuarioModificacion)
                .MaximumLength(100).WithMessage("El usuario de modificación no puede exceder 100 caracteres");
        }

        private async Task<bool> BeUniqueNombre(int categoriaId, string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return true;

            return !await _databaseService.Categoria
                .AnyAsync(c => c.Nombre == nombre && 
                              c.CategoriaID != categoriaId && 
                              c.Estado == true);
        }
    }
}
