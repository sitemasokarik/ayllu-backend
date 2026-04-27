namespace DcodePe.Catering.Application.Validators.Customer
{
    public class UpdateCustomerValidator: AbstractValidator<UpdateCustomerModel>
    {
        public UpdateCustomerValidator() {


            RuleFor(x => x.CustomerId)
                .NotNull()
                .NotEmpty()
                .WithMessage("El id del usuario es requerido")
                .GreaterThan(0);

            RuleFor(x => x.FullName)
                .NotNull()
                .NotEmpty()
                .WithMessage("El id del usuario es requerido")
                .MaximumLength(50);

            RuleFor(x => x.DocumentNumber)
               .NotNull()
               .NotEmpty()
               .WithMessage("El id del usuario es requerido")
               .MaximumLength(8);
        }
    }
}
