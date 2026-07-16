using Microsoft.EntityFrameworkCore;



namespace DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetByClientePortal

{

    public class GetCotizacionesByClientePortalQuery(IDataBaseService databaseService) : IGetCotizacionesByClientePortalQuery

    {

        private readonly IDataBaseService _databaseService = databaseService;



        public async Task<List<GetCotizacionPortalModel>> Execute(int clienteId)

        {

            await RepairInconsistentStatesAsync(clienteId);

            var cotizaciones = await _databaseService.Cotizacion

                .AsNoTracking()

                .Where(c => c.Estado == true && c.ClienteID == clienteId && c.EstadoCotizacion != "Anulado")

                .Include(c => c.Local)

                .Include(c => c.Evento)

                .ToListAsync();



            var cotizacionIds = cotizaciones.Select(c => c.CotizacionID).ToList();

            var vouchers = await _databaseService.PagoVoucher

                .AsNoTracking()

                .Where(v => v.Estado == true && cotizacionIds.Contains(v.CotizacionID))

                .OrderByDescending(v => v.FechaCreacion)

                .ToListAsync();



            var ultimoVoucherPorCotizacion = vouchers

                .GroupBy(v => v.CotizacionID)

                .ToDictionary(g => g.Key, g => g.First());



            var result = cotizaciones.Select(c =>

            {

                ultimoVoucherPorCotizacion.TryGetValue(c.CotizacionID, out var voucher);

                var (estadoCotizacion, voucherPendiente, puedePagar) = ResolvePortalEstado(c.EstadoCotizacion, voucher);



                return new GetCotizacionPortalModel

                {

                    CotizacionID = c.CotizacionID,

                    LocalID = c.LocalID,

                    FechaTentativa = c.FechaTentativa,

                    FechaTentativaOpcional = c.FechaTentativaOpcional,

                    FechaReservada = c.FechaReservada,

                    NumeroInvitados = c.NumeroInvitados,

                    TotalCotizacion = c.TotalCotizacion,

                    TotalEvento = c.TotalEvento,

                    EstadoCotizacion = estadoCotizacion,

                    LocalNombre = c.Local.Nombre,

                    EventoNombre = c.Evento?.Nombre,

                    FechaCreacion = c.FechaCreacion,

                    PuedePagar = puedePagar,

                    VoucherPendiente = voucherPendiente,

                    UltimoVoucherEstado = voucher?.EstadoPago,

                    UltimoVoucherUrl = voucher?.ArchivoUrl,

                    UltimoVoucherObservacionAdmin = voucher?.EstadoPago == "Rechazado"

                        ? voucher.ObservacionAdmin

                        : null

                };

            });



            return result

                .OrderBy(c => GetEstadoPriority(c.EstadoCotizacion))

                .ThenByDescending(c => c.FechaCreacion)

                .ToList();

        }



        internal static (string EstadoCotizacion, bool VoucherPendiente, bool PuedePagar) ResolvePortalEstado(

            string? estadoDb,

            Domain.Entities.Pagos.PagoVoucherEntity? voucher)

        {

            var estado = estadoDb ?? string.Empty;

            var voucherPendiente = voucher?.EstadoPago == "Pendiente";



            if (voucher != null)

            {

                estado = voucher.EstadoPago switch

                {

                    "Pendiente" => "Pendiente",

                    "Rechazado" => "Activo",

                    "Aprobado" => "Evento",

                    _ => estado

                };



                if (voucher.EstadoPago == "Pendiente" && estadoDb != "Pendiente" && estadoDb != "Evento")

                    estado = "Pendiente";

            }



            var puedePagar = estado == "Activo" && !voucherPendiente;

            return (estado, voucherPendiente, puedePagar);

        }



        private async Task RepairInconsistentStatesAsync(int clienteId)
        {
            var cotizaciones = await _databaseService.Cotizacion
                .Where(c => c.Estado == true && c.ClienteID == clienteId && c.EstadoCotizacion == "Pendiente")
                .ToListAsync();

            if (cotizaciones.Count == 0)
                return;

            var ids = cotizaciones.Select(c => c.CotizacionID).ToList();
            var vouchers = await _databaseService.PagoVoucher
                .Where(v => v.Estado == true && ids.Contains(v.CotizacionID))
                .OrderByDescending(v => v.FechaCreacion)
                .ToListAsync();

            var changed = false;
            foreach (var cotizacion in cotizaciones)
            {
                var ultimo = vouchers.FirstOrDefault(v => v.CotizacionID == cotizacion.CotizacionID);
                if (ultimo?.EstadoPago == "Rechazado")
                {
                    cotizacion.EstadoCotizacion = "Activo";
                    cotizacion.FechaModificacion = DateTime.Now;
                    changed = true;
                }
            }

            if (changed)
                await _databaseService.SaveAsync();
        }

        private static int GetEstadoPriority(string estado)

        {

            return estado switch

            {

                "Evento" => 0,

                "Pendiente" => 1,

                "Activo" => 2,

                _ => 3

            };

        }

    }

}

