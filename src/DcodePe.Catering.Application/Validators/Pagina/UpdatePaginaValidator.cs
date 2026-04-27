using FluentValidation;
using DcodePe.Catering.Application.DataBase.Pagina.Commands.Update;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.Validators.Pagina
{
    public class UpdatePaginaValidator : AbstractValidator<UpdatePaginaModel>
    {
        private readonly IDataBaseService _databaseService;

        public UpdatePaginaValidator(IDataBaseService databaseService)
        {
            _databaseService = databaseService;

            RuleFor(x => x.PaginaID)
                .GreaterThan(0).WithMessage("El ID de la página debe ser válido");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre de la página es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres")
                .MustAsync(async (model, nombre, cancellation) => await BeUniqueNombre(model.PaginaID, nombre))
                .WithMessage("Ya existe otra página con ese nombre");

            RuleFor(x => x.Descripcion)
                .MaximumLength(255).WithMessage("La descripción no puede exceder 255 caracteres");

            RuleFor(x => x.Url)
                .MaximumLength(500).WithMessage("La URL no puede exceder 500 caracteres");

            RuleFor(x => x.Icono)
                .MaximumLength(100).WithMessage("El icono no puede exceder 100 caracteres");

            RuleFor(x => x.UsuarioModificacion)
                .NotEmpty().WithMessage("El usuario de modificación es obligatorio")
                .MaximumLength(100).WithMessage("El usuario de modificación no puede exceder 100 caracteres");
        }

        private async Task<bool> BeUniqueNombre(int paginaId, string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return true;

            return !await _databaseService.Pagina
                .AnyAsync(p => p.Nombre == nombre && p.PaginaID != paginaId && p.Estado == true);
        }
    }
}
