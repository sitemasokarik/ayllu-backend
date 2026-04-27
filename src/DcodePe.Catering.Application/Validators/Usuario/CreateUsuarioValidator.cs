using FluentValidation;
using DcodePe.Catering.Application.DataBase.Usuario.Commands.Create;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.Validators.Usuario
{
    public class CreateUsuarioValidator : AbstractValidator<CreateUsuarioModel>
    {
        private readonly IDataBaseService _databaseService;

        public CreateUsuarioValidator(IDataBaseService databaseService)
        {
            _databaseService = databaseService;

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio")
                .MinimumLength(3).WithMessage("El nombre de usuario debe tener al menos 3 caracteres")
                .MaximumLength(100).WithMessage("El nombre de usuario no puede exceder 100 caracteres")
                .MustAsync(async (username, cancellation) => await BeUniqueUserName(username))
                .WithMessage("El nombre de usuario ya está en uso");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio")
                .EmailAddress().WithMessage("El email no es válido")
                .MaximumLength(150).WithMessage("El email no puede exceder 150 caracteres")
                .MustAsync(async (email, cancellation) => await BeUniqueEmail(email))
                .WithMessage("El email ya está en uso");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres")
                .MaximumLength(100).WithMessage("La contraseña no puede exceder 100 caracteres")
                .Matches(@"[A-Z]").WithMessage("La contraseña debe contener al menos una mayúscula")
                .Matches(@"[a-z]").WithMessage("La contraseña debe contener al menos una minúscula")
                .Matches(@"[0-9]").WithMessage("La contraseña debe contener al menos un número")
                .Matches(@"[\W_]").WithMessage("La contraseña debe contener al menos un carácter especial");

            RuleFor(x => x.RolID)
                .GreaterThan(0).WithMessage("Debe seleccionar un rol válido");

            RuleFor(x => x.UsuarioCreacion)
                .MaximumLength(100).WithMessage("El usuario de creación no puede exceder 100 caracteres");
        }

        private async Task<bool> BeUniqueUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return true;

            return !await _databaseService.Usuario
                .AnyAsync(u => u.UserName == userName && u.Estado == true);
        }

        private async Task<bool> BeUniqueEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true;

            return !await _databaseService.Usuario
                .AnyAsync(u => u.Email == email && u.Estado == true);
        }
    }
}
