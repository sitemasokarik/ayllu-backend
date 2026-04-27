using FluentValidation;
using DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Update;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.Validators.Cotizacion
{
    public class UpdateCotizacionValidator : AbstractValidator<UpdateCotizacionModel>
    {
        private readonly IDataBaseService _databaseService;

        public UpdateCotizacionValidator(IDataBaseService databaseService)
        {
            _databaseService = databaseService;

            RuleFor(x => x.CotizacionID)
                .GreaterThan(0).WithMessage("El ID de la cotización debe ser válido");

            RuleFor(x => x.ClienteID)
                .GreaterThan(0).WithMessage("Debe seleccionar un cliente válido")
                .MustAsync(async (clienteId, cancellation) => await ClienteExists(clienteId))
                .WithMessage("El cliente seleccionado no existe o está inactivo");

            RuleFor(x => x.LocalID)
                .GreaterThan(0).WithMessage("Debe seleccionar un local válido")
                .MustAsync(async (localId, cancellation) => await LocalExists(localId))
                .WithMessage("El local seleccionado no existe o está inactivo");

            RuleFor(x => x.EventoID)
                .MustAsync(async (eventoId, cancellation) => await EventoExists(eventoId))
                .WithMessage("El evento seleccionado no existe o está inactivo")
                .When(x => x.EventoID.HasValue && x.EventoID > 0);

            RuleFor(x => x.FechaTentativa)
                .Must(fecha => fecha == null || fecha >= DateTime.Today)
                .WithMessage("La fecha tentativa no puede ser anterior a hoy")
                .When(x => x.FechaTentativa.HasValue);

            RuleFor(x => x.FechaTentativaOpcional)
                .Must(fecha => fecha == null || fecha >= DateTime.Today)
                .WithMessage("La fecha tentativa opcional no puede ser anterior a hoy")
                .When(x => x.FechaTentativaOpcional.HasValue);

            RuleFor(x => x.NumeroInvitados)
                .GreaterThan(0).WithMessage("El número de invitados debe ser mayor a 0");

            RuleFor(x => x.CostoDePersonal)
                .GreaterThanOrEqualTo(0).WithMessage("El costo de personal debe ser mayor o igual a 0");

            RuleFor(x => x.Garantia)
                .GreaterThanOrEqualTo(0).WithMessage("La garantía debe ser mayor o igual a 0");

            RuleFor(x => x.TarifaMenuPorInvitado)
                .GreaterThanOrEqualTo(0).WithMessage("La tarifa del menú por invitado debe ser mayor o igual a 0");

            RuleFor(x => x.SubtotalMenu)
                .GreaterThanOrEqualTo(0).WithMessage("El subtotal del menú debe ser mayor o igual a 0");

            RuleFor(x => x.TotalEvento)
                .GreaterThanOrEqualTo(0).WithMessage("El total del evento debe ser mayor o igual a 0");

            RuleFor(x => x.PrecioPorCubierto)
                .GreaterThanOrEqualTo(0).WithMessage("El precio por cubierto debe ser mayor o igual a 0");

            RuleFor(x => x.PrecioPorCubiertoConDescuento)
                .GreaterThanOrEqualTo(0).WithMessage("El precio por cubierto con descuento debe ser mayor o igual a 0");

            RuleFor(x => x.TotalCotizacion)
                .GreaterThanOrEqualTo(0).WithMessage("El total de la cotización debe ser mayor o igual a 0");

            RuleFor(x => x.EstadoCotizacion)
                .NotEmpty().WithMessage("El estado de la cotización es obligatorio")
                .Must(estado => new[] { "Activo", "Anulado", "Evento" }.Contains(estado))
                .WithMessage("El estado de la cotización debe ser: Activo, Anulado o Evento");

            RuleFor(x => x.Observacion)
                .MaximumLength(1000).WithMessage("La observación no puede exceder 1000 caracteres");

            RuleFor(x => x.UsuarioModificacion)
                .NotEmpty().WithMessage("El usuario de modificación es obligatorio")
                .MaximumLength(100).WithMessage("El usuario de modificación no puede exceder 100 caracteres");

            // Validaciones para CotizacionProducto
            RuleForEach(x => x.CotizacionProducto)
                .ChildRules(producto =>
                {
                    producto.RuleFor(p => p.ProductoID)
                        .GreaterThan(0).WithMessage("El ID del producto debe ser válido")
                        .MustAsync(async (productoId, cancellation) => await ProductoExists(productoId))
                        .WithMessage("El producto seleccionado no existe o está inactivo");

                    producto.RuleFor(p => p.Cantidad)
                        .GreaterThan(0).WithMessage("La cantidad del producto debe ser mayor a 0");
                })
                .When(x => x.CotizacionProducto != null && x.CotizacionProducto.Any());

            // Validaciones para CotizacionServicio
            RuleForEach(x => x.CotizacionServicio)
                .ChildRules(servicio =>
                {
                    servicio.RuleFor(s => s.ServicioID)
                        .GreaterThan(0).WithMessage("El ID del servicio debe ser válido")
                        .MustAsync(async (servicioId, cancellation) => await ServicioExists(servicioId))
                        .WithMessage("El servicio seleccionado no existe o está inactivo");

                    servicio.RuleFor(s => s.Cantidad)
                        .GreaterThan(0).WithMessage("La cantidad del servicio debe ser mayor a 0");
                })
                .When(x => x.CotizacionServicio != null && x.CotizacionServicio.Any());
        }

        private async Task<bool> ClienteExists(int clienteId)
        {
            return await _databaseService.Cliente
                .AnyAsync(c => c.ClienteID == clienteId && c.Estado == true);
        }

        private async Task<bool> LocalExists(int localId)
        {
            return await _databaseService.Local
                .AnyAsync(l => l.LocalID == localId && l.Estado == true);
        }

        private async Task<bool> EventoExists(int? eventoId)
        {
            if (!eventoId.HasValue || eventoId == 0)
                return true;

            return await _databaseService.Evento
                .AnyAsync(e => e.EventoID == eventoId && e.Estado == true);
        }

        private async Task<bool> ProductoExists(int productoId)
        {
            return await _databaseService.Producto
                .AnyAsync(p => p.ProductoID == productoId && p.Estado == true);
        }

        private async Task<bool> ServicioExists(int servicioId)
        {
            return await _databaseService.ServicioAdicional
                .AnyAsync(s => s.ServicioID == servicioId && s.Estado == true);
        }
    }
}
