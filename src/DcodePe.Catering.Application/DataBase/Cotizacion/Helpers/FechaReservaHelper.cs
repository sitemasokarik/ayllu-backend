using DcodePe.Catering.Domain.Entities;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Helpers
{
    public static class FechaReservaHelper
    {
        public static bool TieneDosFechasTentativas(CotizacionEntity cotizacion) =>
            cotizacion.FechaTentativa.HasValue && cotizacion.FechaTentativaOpcional.HasValue;

        public static IReadOnlyList<DateTime> ObtenerFechasDisponibles(CotizacionEntity cotizacion)
        {
            var fechas = new List<DateTime>();
            if (cotizacion.FechaTentativa.HasValue)
                fechas.Add(cotizacion.FechaTentativa.Value.Date);
            if (cotizacion.FechaTentativaOpcional.HasValue
                && !fechas.Contains(cotizacion.FechaTentativaOpcional.Value.Date))
                fechas.Add(cotizacion.FechaTentativaOpcional.Value.Date);
            return fechas;
        }

        public static DateTime ResolverFechaReservada(CotizacionEntity cotizacion, DateTime? fechaElegida)
        {
            var disponibles = ObtenerFechasDisponibles(cotizacion);
            if (disponibles.Count == 0)
                throw new InvalidOperationException("La cotización no tiene fechas tentativas registradas.");

            if (disponibles.Count == 1)
                return disponibles[0];

            if (!fechaElegida.HasValue)
                throw new InvalidOperationException("Debes elegir la fecha que deseas reservar con el adelanto.");

            var elegida = fechaElegida.Value.Date;
            if (!disponibles.Any(f => f == elegida))
                throw new InvalidOperationException("La fecha elegida no coincide con las fechas tentativas de tu cotización.");

            return elegida;
        }
    }
}
