using FluentValidation;
using DcodePe.Catering.Application.DataBase.Usuario.Commands.UpdatePassword;

namespace DcodePe.Catering.Application.Validators.Usuario
{
    public class UpdateUsuarioPasswordValidator : AbstractValidator<UpdateUsuarioPasswordModel>
    {
        public UpdateUsuarioPasswordValidator()
        {
            RuleFor(x => x.UsuarioID)
                .GreaterThan(0).WithMessage("El ID del usuario es obligatorio");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("La contraseña actual es obligatoria");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("La nueva contraseña es obligatoria")
                .MinimumLength(8).WithMessage("La nueva contraseña debe tener al menos 8 caracteres")
                .MaximumLength(100).WithMessage("La nueva contraseña no puede exceder 100 caracteres")
                .Matches(@"[A-Z]").WithMessage("La nueva contraseña debe contener al menos una mayúscula")
                .Matches(@"[a-z]").WithMessage("La nueva contraseña debe contener al menos una minúscula")
                .Matches(@"[0-9]").WithMessage("La nueva contraseña debe contener al menos un número")
                .Matches(@"[\W_]").WithMessage("La nueva contraseña debe contener al menos un carácter especial");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("La confirmación de contraseña es obligatoria")
                .Equal(x => x.NewPassword).WithMessage("Las contraseñas no coinciden");

            RuleFor(x => x.NewPassword)
                .NotEqual(x => x.CurrentPassword)
                .When(x => !string.IsNullOrEmpty(x.CurrentPassword))
                .WithMessage("La nueva contraseña debe ser diferente a la actual");
        }
    }
}
