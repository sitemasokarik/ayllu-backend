using Microsoft.EntityFrameworkCore;



namespace DcodePe.Catering.Application.DataBase.Cotizacion.Helpers

{

    /// <summary>

    /// Solo bloquea la fecha final del evento (FechaReservada), elegida al pagar el adelanto.

    /// Nunca bloquea las dos fechas tentativas del presupuesto.

    /// </summary>

    public static class FechaReservadaHelper

    {

        public static async Task<List<string>> GetFechasReservadasAsync(

            IDataBaseService db,

            int localId,

            int? excludeCotizacionId = null)

        {

            await SincronizarFechasReservadasLocalAsync(db, localId);



            var query = db.Cotizacion.AsNoTracking()

                .Where(c =>

                    c.Estado == true &&

                    c.LocalID == localId &&

                    c.EstadoCotizacion == "Evento" &&

                    c.FechaReservada != null);



            if (excludeCotizacionId.HasValue)

                query = query.Where(c => c.CotizacionID != excludeCotizacionId.Value);



            var fechas = await query

                .Select(c => c.FechaReservada!.Value.Date)

                .Distinct()

                .ToListAsync();



            return fechas

                .Select(d => d.ToString("yyyy-MM-dd"))

                .OrderBy(x => x)

                .ToList();

        }



        public static async Task<bool> FechaEstaReservadaAsync(

            IDataBaseService db,

            int localId,

            DateTime? fecha1,

            DateTime? fecha2,

            int? excludeCotizacionId = null)

        {

            var reservadas = await GetFechasReservadasAsync(db, localId, excludeCotizacionId);

            if (!reservadas.Any())

                return false;



            var f1 = ToDateKey(fecha1);

            var f2 = ToDateKey(fecha2);

            return (f1 != null && reservadas.Contains(f1))

                || (f2 != null && reservadas.Contains(f2));

        }



        /// <summary>

        /// Copia FechaReservadaElegida del pago aprobado a Cotizacion.FechaReservada cuando falta.

        /// </summary>

        public static async Task SincronizarFechasReservadasLocalAsync(IDataBaseService db, int localId)

        {

            var pendientes = await db.Cotizacion

                .Where(c =>

                    c.Estado == true &&

                    c.LocalID == localId &&

                    c.EstadoCotizacion == "Evento" &&

                    c.FechaReservada == null)

                .Select(c => c.CotizacionID)

                .ToListAsync();



            if (pendientes.Count == 0)

                return;



            var voucherFechas = await db.PagoVoucher

                .Where(v =>

                    v.Estado == true &&

                    v.EstadoPago == "Aprobado" &&

                    v.FechaReservadaElegida != null &&

                    pendientes.Contains(v.CotizacionID))

                .GroupBy(v => v.CotizacionID)

                .Select(g => new

                {

                    CotizacionID = g.Key,

                    Fecha = g.OrderByDescending(v => v.PagoVoucherID).First().FechaReservadaElegida!.Value

                })

                .ToListAsync();



            var mpFechas = await db.PagoMercadoPago

                .Where(p =>

                    p.Estado == true &&

                    p.EstadoPago == "Aprobado" &&

                    p.FechaReservadaElegida != null &&

                    pendientes.Contains(p.CotizacionID))

                .GroupBy(p => p.CotizacionID)

                .Select(g => new

                {

                    CotizacionID = g.Key,

                    Fecha = g.OrderByDescending(p => p.PagoMercadoPagoID).First().FechaReservadaElegida!.Value

                })

                .ToListAsync();



            var fechasPorCotizacion = voucherFechas

                .Concat(mpFechas)

                .GroupBy(x => x.CotizacionID)

                .ToDictionary(

                    g => g.Key,

                    g => g.OrderByDescending(x => x.Fecha).First().Fecha.Date);



            if (fechasPorCotizacion.Count == 0)

                return;



            var cotizaciones = await db.Cotizacion

                .Where(c => pendientes.Contains(c.CotizacionID))

                .ToListAsync();



            var changed = false;

            foreach (var cotizacion in cotizaciones)

            {

                if (!fechasPorCotizacion.TryGetValue(cotizacion.CotizacionID, out var fecha))

                    continue;



                cotizacion.FechaReservada = fecha;

                cotizacion.FechaModificacion = DateTime.Now;

                cotizacion.UsuarioModificacion ??= "sync-fecha-reservada";

                changed = true;

            }



            if (changed)

                await db.SaveAsync();

        }



        internal static DateTime? ResolverFechaEventoConfirmada(

            DateTime? fechaReservada,

            DateTime? fechaVoucher,

            DateTime? fechaMercadoPago,

            DateTime? fechaTentativa,

            DateTime? fechaTentativaOpcional)

        {

            if (fechaReservada.HasValue)

                return fechaReservada.Value.Date;

            if (fechaVoucher.HasValue)

                return fechaVoucher.Value.Date;

            if (fechaMercadoPago.HasValue)

                return fechaMercadoPago.Value.Date;

            if (fechaTentativa.HasValue && !fechaTentativaOpcional.HasValue)

                return fechaTentativa.Value.Date;

            return null;

        }



        private static string? ToDateKey(DateTime? value)

        {

            if (!value.HasValue)

                return null;

            return value.Value.Date.ToString("yyyy-MM-dd");

        }

    }

}


