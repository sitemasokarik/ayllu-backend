using DcodePe.Catering.Application.DataBase.Cotizacion.Helpers;
using DcodePe.Catering.Domain.Entities.Pagos;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.PagoVoucher.Commands.UploadPortal
{
    public class UploadPagoVoucherPortalCommand(IDataBaseService databaseService) : IUploadPagoVoucherPortalCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;

        public async Task<UploadPagoVoucherPortalModel> Execute(UploadPagoVoucherPortalModel model)
        {
            var cotizacion = await _databaseService.Cotizacion
                .FirstOrDefaultAsync(c =>
                    c.CotizacionID == model.CotizacionID &&
                    c.ClienteID == model.ClienteID &&
                    c.Estado == true);

            if (cotizacion == null)
                throw new InvalidOperationException("Cotización no encontrada.");

            var ultimoVoucher = await _databaseService.PagoVoucher
                .Where(v => v.CotizacionID == model.CotizacionID && v.Estado == true)
                .OrderByDescending(v => v.FechaCreacion)
                .FirstOrDefaultAsync();

            if (ultimoVoucher?.EstadoPago == "Pendiente")
                throw new InvalidOperationException("Ya existe un voucher pendiente de revisión para esta cotización.");

            if (cotizacion.EstadoCotizacion == "Pendiente" && ultimoVoucher?.EstadoPago == "Rechazado")
            {
                cotizacion.EstadoCotizacion = "Activo";
                cotizacion.UsuarioModificacion = model.UsuarioCreacion;
                cotizacion.FechaModificacion = DateTime.Now;
            }
            else if (cotizacion.EstadoCotizacion != "Activo")
            {
                throw new InvalidOperationException("Solo puedes pagar cotizaciones en estado Activo.");
            }

            var empresa = await _databaseService.Empresa
                .Where(e => e.Estado == true)
                .OrderBy(e => e.EmpresaID)
                .FirstOrDefaultAsync();

            var montoMinimoAdelanto = empresa?.MontoAdelantoReserva > 0
                ? empresa.MontoAdelantoReserva.Value
                : 1000m;

            if (Math.Abs(model.Monto - montoMinimoAdelanto) > 0.01m)
            {
                throw new InvalidOperationException(
                    $"El adelanto debe ser exactamente S/ {montoMinimoAdelanto:N2}.");
            }

            var fechaReservada = FechaReservaHelper.ResolverFechaReservada(cotizacion, model.FechaReservadaElegida);

            var entity = new PagoVoucherEntity
            {
                CotizacionID = model.CotizacionID,
                ClienteID = model.ClienteID,
                ArchivoUrl = model.ArchivoUrl,
                NombreArchivo = model.NombreArchivo,
                Monto = montoMinimoAdelanto,
                FechaReservadaElegida = fechaReservada,
                ObservacionCliente = model.ObservacionCliente,
                EstadoPago = "Pendiente",
                UsuarioCreacion = model.UsuarioCreacion,
                FechaCreacion = DateTime.Now,
                Estado = true
            };

            await _databaseService.PagoVoucher.AddAsync(entity);

            cotizacion.EstadoCotizacion = "Pendiente";
            cotizacion.UsuarioModificacion = model.UsuarioCreacion;
            cotizacion.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return model;
        }
    }
}
