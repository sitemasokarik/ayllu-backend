using Microsoft.Extensions.Logging;

namespace DcodePe.Catering.Persistence.DataBase
{
    public static class DevCotizacionCleanup
    {
        public static void ClearAllCotizaciones(DataBaseService db, ILogger logger) =>
            ProductionDataCleanup.ClearCotizacionesOnly(db, logger);
    }
}
