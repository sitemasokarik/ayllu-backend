using FluentValidation;
using DcodePe.Catering.Application.DataBase.Usuario.Commands.Update;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.Validators.Usuario
{
    public class UpdateUsuarioValidator : AbstractValidator<UpdateUsuarioModel>
    {
        private readonly IDataBaseService _databaseService;

        public UpdateUsuarioValidator(IDataBaseService databaseService)
        {
            _databaseService = databaseService;

            RuleFor(x => x.UsuarioID)
                .GreaterThan(0).WithMessage("El ID del usuario es obligatorio");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio")
                .MinimumLength(3).WithMessage("El nombre de usuario debe tener al menos 3 caracteres")
                .MaximumLength(100).WithMessage("El nombre de usuario no puede exceder 100 caracteres")
                .MustAsync(async (model, username, cancellation) => 
                    await BeUniqueUserName(model.UsuarioID, username))
                .WithMessage("El nombre de usuario ya está en uso");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio")
                .EmailAddress().WithMessage("El email no es válido")
                .MaximumLength(150).WithMessage("El email no puede exceder 150 caracteres")
                .MustAsync(async (model, email, cancellation) => 
                    await BeUniqueEmail(model.UsuarioID, email))
                .WithMessage("El email ya está en uso");

            RuleFor(x => x.RolID)
                .GreaterThan(0).WithMessage("Debe seleccionar un rol válido");

            RuleFor(x => x.UsuarioModificacion)
                .MaximumLength(100).WithMessage("El usuario de modificación no puede exceder 100 caracteres");
        }

        private async Task<bool> BeUniqueUserName(int usuarioId, string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return true;

            return !await _databaseService.Usuario
                .AnyAsync(u => u.UserName == userName && 
                              u.UsuarioID != usuarioId && 
                              u.Estado == true);
        }

        private async Task<bool> BeUniqueEmail(int usuarioId, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true;

            return !await _databaseService.Usuario
                .AnyAsync(u => u.Email == email && 
                              u.UsuarioID != usuarioId && 
                              u.Estado == true);
        }
    }
}
