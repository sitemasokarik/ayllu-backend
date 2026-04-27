using FluentValidation;

namespace DcodePe.Catering.Application.Validators.Usuario
{
    public class LoginUsuarioValidator : AbstractValidator<(string UserName, string Password)>
    {
        public LoginUsuarioValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("El nombre de usuario o email es obligatorio")
                .MinimumLength(3).WithMessage("El nombre de usuario debe tener al menos 3 caracteres")
                .MaximumLength(100).WithMessage("El nombre de usuario no puede exceder 100 caracteres");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres")
                .MaximumLength(100).WithMessage("La contraseña no puede exceder 100 caracteres");
        }
    }
}
