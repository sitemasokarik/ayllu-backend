using FluentValidation;
using DcodePe.Catering.Application.DataBase.Rol.Commands.Create;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.Validators.Rol
{
    public class CreateRolValidator : AbstractValidator<CreateRolModel>
    {
        private readonly IDataBaseService _databaseService;

        public CreateRolValidator(IDataBaseService databaseService)
        {
            _databaseService = databaseService;

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del rol es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres")
                .MustAsync(async (nombre, cancellation) => await BeUniqueNombre(nombre))
                .WithMessage("Ya existe un rol con ese nombre");

            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");

            RuleFor(x => x.UsuarioCreacion)
                .NotEmpty().WithMessage("El usuario de creación es obligatorio")
                .MaximumLength(100).WithMessage("El usuario de creación no puede exceder 100 caracteres");
        }

        private async Task<bool> BeUniqueNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return true;

            return !await _databaseService.Rol
                .AnyAsync(r => r.Nombre == nombre && r.Estado == true);
        }
    }
}
