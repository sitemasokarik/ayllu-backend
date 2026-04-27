using FluentValidation;
using DcodePe.Catering.Application.DataBase.Pagina.Commands.Create;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.Validators.Pagina
{
    public class CreatePaginaValidator : AbstractValidator<CreatePaginaModel>
    {
        private readonly IDataBaseService _databaseService;

        public CreatePaginaValidator(IDataBaseService databaseService)
        {
            _databaseService = databaseService;

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre de la página es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres")
                .MustAsync(async (nombre, cancellation) => await BeUniqueNombre(nombre))
                .WithMessage("Ya existe una página con ese nombre");

            RuleFor(x => x.Descripcion)
                .MaximumLength(255).WithMessage("La descripción no puede exceder 255 caracteres");

            RuleFor(x => x.Url)
                .MaximumLength(500).WithMessage("La URL no puede exceder 500 caracteres");

            RuleFor(x => x.Icono)
                .MaximumLength(100).WithMessage("El icono no puede exceder 100 caracteres");

            RuleFor(x => x.UsuarioCreacion)
                .NotEmpty().WithMessage("El usuario de creación es obligatorio")
                .MaximumLength(100).WithMessage("El usuario de creación no puede exceder 100 caracteres");
        }

        private async Task<bool> BeUniqueNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return true;

            return !await _databaseService.Pagina
                .AnyAsync(p => p.Nombre == nombre && p.Estado == true);
        }
    }
}
