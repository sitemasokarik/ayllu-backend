using FluentValidation;
using DcodePe.Catering.Application.DataBase.Cliente.Commands.Update;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.Validators.Cliente
{
    public class UpdateClienteValidator : AbstractValidator<UpdateClienteModel>
    {
        private readonly IDataBaseService _databaseService;

        public UpdateClienteValidator(IDataBaseService databaseService)
        {
            _databaseService = databaseService;

            RuleFor(x => x.ClienteID)
                .GreaterThan(0).WithMessage("El ID del cliente es obligatorio");

            RuleFor(x => x.TipoDocumento)
                .NotEmpty().WithMessage("El tipo de documento es obligatorio")
                .MaximumLength(50).WithMessage("El tipo de documento no puede exceder 50 caracteres")
                .Must(BeValidTipoDocumento).WithMessage("Tipo de documento no válido. Use: DNI, RUC, Pasaporte, Carnet de Extranjería");

            RuleFor(x => x.NumeroDocumento)
                .NotEmpty().WithMessage("El número de documento es obligatorio")
                .MaximumLength(20).WithMessage("El número de documento no puede exceder 20 caracteres")
                .MustAsync(async (model, numeroDocumento, cancellation) => 
                    await BeUniqueNumeroDocumento(model.ClienteID, numeroDocumento))
                .WithMessage("Ya existe un cliente con ese número de documento");

            RuleFor(x => x.NombreCompleto)
                .NotEmpty().WithMessage("El nombre completo es obligatorio")
                .MaximumLength(200).WithMessage("El nombre completo no puede exceder 200 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio")
                .EmailAddress().WithMessage("El email no tiene un formato válido")
                .MaximumLength(100).WithMessage("El email no puede exceder 100 caracteres");

            RuleFor(x => x.Telefono)
                .NotEmpty().WithMessage("El teléfono es obligatorio")
                .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres");

            RuleFor(x => x.TelefonoSecundario)
                .MaximumLength(20).WithMessage("El teléfono secundario no puede exceder 20 caracteres");

            RuleFor(x => x.Direccion)
                .MaximumLength(500).WithMessage("La dirección no puede exceder 500 caracteres");

            RuleFor(x => x.Ciudad)
                .MaximumLength(100).WithMessage("La ciudad no puede exceder 100 caracteres");

            RuleFor(x => x.Pais)
                .MaximumLength(100).WithMessage("El país no puede exceder 100 caracteres");

            RuleFor(x => x.TipoCliente)
                .MaximumLength(50).WithMessage("El tipo de cliente no puede exceder 50 caracteres")
                .Must(BeValidTipoCliente).WithMessage("Tipo de cliente no válido. Use: Particular, Empresa, Gobierno");

            RuleFor(x => x.Observaciones)
                .MaximumLength(1000).WithMessage("Las observaciones no pueden exceder 1000 caracteres");

            RuleFor(x => x.UsuarioModificacion)
                .NotEmpty().WithMessage("El usuario de modificación es obligatorio")
                .MaximumLength(100).WithMessage("El usuario de modificación no puede exceder 100 caracteres");
        }

        private bool BeValidTipoDocumento(string tipoDocumento)
        {
            if (string.IsNullOrWhiteSpace(tipoDocumento))
                return true;

            var tiposValidos = new[] { "DNI", "RUC", "Pasaporte", "Carnet de Extranjería", "CE" };
            return tiposValidos.Contains(tipoDocumento, StringComparer.OrdinalIgnoreCase);
        }

        private bool BeValidTipoCliente(string tipoCliente)
        {
            if (string.IsNullOrWhiteSpace(tipoCliente))
                return true;

            var tiposValidos = new[] { "Particular", "Empresa", "Gobierno" };
            return tiposValidos.Contains(tipoCliente, StringComparer.OrdinalIgnoreCase);
        }

        private async Task<bool> BeUniqueNumeroDocumento(int clienteId, string numeroDocumento)
        {
            if (string.IsNullOrWhiteSpace(numeroDocumento))
                return true;

            return !await _databaseService.Cliente
                .AnyAsync(c => c.NumeroDocumento == numeroDocumento &&
                              c.ClienteID != clienteId &&
                              c.Estado == true);
        }
    }
}
