using Microsoft.EntityFrameworkCore;
using DcodePe.Catering.Application.DataBase.Cotizacion.Helpers;

namespace DcodePe.Catering.Application.DataBase.PagoVoucher.Commands.Review
{
    public class ReviewPagoVoucherCommand(IDataBaseService databaseService) : IReviewPagoVoucherCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;

        public async Task<bool> Execute(ReviewPagoVoucherModel model)
        {
            var voucher = await _databaseService.PagoVoucher
                .FirstOrDefaultAsync(v => v.PagoVoucherID == model.PagoVoucherID && v.Estado == true);

            if (voucher == null)
                return false;

            if (voucher.EstadoPago != "Pendiente")
                throw new InvalidOperationException("Este voucher ya fue revisado.");

            var cotizacion = await _databaseService.Cotizacion
                .FirstOrDefaultAsync(c => c.CotizacionID == voucher.CotizacionID && c.Estado == true);

            if (cotizacion == null)
                throw new InvalidOperationException("Cotización asociada no encontrada.");

            voucher.EstadoPago = model.Aprobado ? "Aprobado" : "Rechazado";
            voucher.ObservacionAdmin = model.ObservacionAdmin;
            voucher.UsuarioModificacion = model.UsuarioModificacion;
            voucher.FechaModificacion = DateTime.Now;

            cotizacion.EstadoCotizacion = model.Aprobado ? "Evento" : "Activo";

            if (model.Aprobado)
            {
                var fechaConfirmada = ResolverFechaAlAprobar(cotizacion, voucher, model.FechaReservadaElegida);
                voucher.FechaReservadaElegida = fechaConfirmada;
                cotizacion.FechaReservada = fechaConfirmada;
            }

            cotizacion.UsuarioModificacion = model.UsuarioModificacion;
            cotizacion.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }

        private static DateTime ResolverFechaAlAprobar(
            Domain.Entities.CotizacionEntity cotizacion,
            Domain.Entities.Pagos.PagoVoucherEntity voucher,
            DateTime? fechaAdmin)
        {
            if (voucher.FechaReservadaElegida.HasValue)
                return voucher.FechaReservadaElegida.Value.Date;

            if (fechaAdmin.HasValue)
                return FechaReservaHelper.ResolverFechaReservada(cotizacion, fechaAdmin).Date;

            var disponibles = FechaReservaHelper.ObtenerFechasDisponibles(cotizacion);
            if (disponibles.Count == 1)
                return disponibles[0];

            throw new InvalidOperationException(
                "Debes indicar la fecha del evento que el cliente confirma con este adelanto (solo una de las fechas tentativas).");
        }
    }
}
