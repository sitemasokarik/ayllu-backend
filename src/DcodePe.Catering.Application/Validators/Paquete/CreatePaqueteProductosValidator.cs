using DcodePe.Catering.Application.DataBase.Paquete.Commands.Create;
using FluentValidation;

namespace DcodePe.Catering.Application.Validators.Paquete
{
    public class CreatePaqueteProductosValidator : AbstractValidator<CreatePaqueteProductosModel>
    {
        public CreatePaqueteProductosValidator()
        {
            // Validación del nombre del paquete
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del paquete es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

            // Validación de la descripción
            RuleFor(x => x.Descripcion)
                .MaximumLength(255).WithMessage("La descripción no puede exceder 255 caracteres");

            // Validación del precio total
            RuleFor(x => x.PrecioTotal)
                .GreaterThan(0).WithMessage("El precio total debe ser mayor a 0");

            // Validación del usuario de creación
            RuleFor(x => x.UsuarioCreacion)
                .NotEmpty().WithMessage("El usuario de creación es obligatorio")
                .MaximumLength(100).WithMessage("El usuario de creación no puede exceder 100 caracteres");

            // Validación: Al menos debe tener productos o servicios
            RuleFor(x => x)
                .Must(x => (x.Productos != null && x.Productos.Any()) || 
                           (x.Servicios != null && x.Servicios.Any()))
                .WithMessage("El paquete debe incluir al menos un producto o un servicio");

            // Validación de productos
            RuleForEach(x => x.Productos).ChildRules(producto =>
            {
                producto.RuleFor(p => p.ProductoID)
                    .GreaterThan(0).WithMessage("El ID del producto debe ser mayor a 0");

                producto.RuleFor(p => p.Cantidad)
                    .GreaterThan(0).WithMessage("La cantidad del producto debe ser mayor a 0");
            });

            // Validación de servicios
            RuleForEach(x => x.Servicios).ChildRules(servicio =>
            {
                servicio.RuleFor(s => s.ServicioID)
                    .GreaterThan(0).WithMessage("El ID del servicio debe ser mayor a 0");

                servicio.RuleFor(s => s.Cantidad)
                    .GreaterThan(0).WithMessage("La cantidad del servicio debe ser mayor a 0");
            });

            // Validación: No puede haber productos duplicados
            RuleFor(x => x.Productos)
                .Must(productos => productos == null || 
                      productos.Select(p => p.ProductoID).Distinct().Count() == productos.Count)
                .When(x => x.Productos != null && x.Productos.Any())
                .WithMessage("No puede agregar el mismo producto más de una vez");

            // Validación: No puede haber servicios duplicados
            RuleFor(x => x.Servicios)
                .Must(servicios => servicios == null || 
                      servicios.Select(s => s.ServicioID).Distinct().Count() == servicios.Count)
                .When(x => x.Servicios != null && x.Servicios.Any())
                .WithMessage("No puede agregar el mismo servicio más de una vez");
        }
    }
}
