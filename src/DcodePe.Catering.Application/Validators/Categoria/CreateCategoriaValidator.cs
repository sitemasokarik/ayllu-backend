using FluentValidation;
using DcodePe.Catering.Application.DataBase.Categoria.Commands.Create;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.Validators.Categoria
{
    public class CreateCategoriaValidator : AbstractValidator<CreateCategoriaModel>
    {
        private readonly IDataBaseService _databaseService;

        public CreateCategoriaValidator(IDataBaseService databaseService)
        {
            _databaseService = databaseService;

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre de la categoría es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres")
                .MustAsync(async (nombre, cancellation) => await BeUniqueNombre(nombre))
                .WithMessage("Ya existe una categoría con ese nombre");

            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");

            RuleFor(x => x.Limite)
                .GreaterThanOrEqualTo(1).WithMessage("El límite debe ser mayor o igual a 1");

            RuleFor(x => x.UsuarioCreacion)
                .MaximumLength(100).WithMessage("El usuario de creación no puede exceder 100 caracteres");
        }

        private async Task<bool> BeUniqueNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return true;

            return !await _databaseService.Categoria
                .AnyAsync(c => c.Nombre == nombre && c.Estado == true);
        }
    }
}
