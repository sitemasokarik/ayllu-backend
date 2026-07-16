using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DcodePe.Catering.Persistence.DataBase
{
    public static class DevEventoTarifasSeeder
    {
        private const string TarifasBodas = """
            {"minInvitados":60,"garantia":500,"tarifas":[{"minInvitados":60,"precioPorInvitado":250},{"minInvitados":80,"precioPorInvitado":240},{"minInvitados":100,"precioPorInvitado":230},{"minInvitados":150,"precioPorInvitado":220},{"minInvitados":170,"precioPorInvitado":210},{"minInvitados":200,"precioPorInvitado":200},{"minInvitados":250,"precioPorInvitado":190}]}
            """;

        private const string TarifasCorporativo = """
            {"minInvitados":150,"garantia":500,"tarifas":[{"minInvitados":150,"precioPorInvitado":150},{"minInvitados":500,"precioPorInvitado":90},{"minInvitados":700,"precioPorInvitado":90}]}
            """;

        public static void EnsureTarifas(DataBaseService db, ILogger logger)
        {
            if (!DataBaseService.UseSqliteProvider)
                return;

            var map = new Dictionary<int, string>
            {
                [1] = TarifasBodas,
                [2] = TarifasBodas,
                [3] = TarifasCorporativo,
                [4] = TarifasBodas,
            };

            foreach (var (eventoId, json) in map)
            {
                var evento = db.Evento.FirstOrDefault(e => e.EventoID == eventoId);
                if (evento == null) continue;

                evento.TarifasInvitadoJson = json;
            }

            db.SaveChanges();
            logger.LogInformation("Tarifas por invitado aplicadas a eventos de desarrollo.");

            ZeroProductPricesForDev(db, logger);
        }

        private static void ZeroProductPricesForDev(DataBaseService db, ILogger logger)
        {
            try
            {
                var updated = db.Database.ExecuteSqlRaw("UPDATE Producto SET Precio = 0");
                logger.LogInformation("Precios de producto en 0 para cotizador por rangos ({Count} filas).", updated);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "No se pudieron poner precios de producto en 0.");
            }
        }
    }
}
