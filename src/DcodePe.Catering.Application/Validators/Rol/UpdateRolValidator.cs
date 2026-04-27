using FluentValidation;
using DcodePe.Catering.Application.DataBase.Rol.Commands.Update;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.Validators.Rol
{
    public class UpdateRolValidator : AbstractValidator<UpdateRolModel>
    {
        private readonly IDataBaseService _databaseService;

        public UpdateRolValidator(IDataBaseService databaseService)
        {
            _databaseService = databaseService;

            RuleFor(x => x.RolID)
                .GreaterThan(0).WithMessage("El ID del rol debe ser válido");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del rol es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres")
                .MustAsync(async (model, nombre, cancellation) => await BeUniqueNombre(model.RolID, nombre))
                .WithMessage("Ya existe un rol con ese nombre");

            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");

            RuleFor(x => x.UsuarioModificacion)
                .NotEmpty().WithMessage("El usuario de modificación es obligatorio")
                .MaximumLength(100).WithMessage("El usuario de modificación no puede exceder 100 caracteres");
        }

        private async Task<bool> BeUniqueNombre(int rolId, string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return true;

            return !await _databaseService.Rol
                .AnyAsync(r => r.Nombre == nombre && r.RolID != rolId && r.Estado == true);
        }
    }
}
