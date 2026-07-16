using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DcodePe.Catering.Persistence.DataBase
{
    /// <summary>
    /// Elimina datos operativos (cotizaciones, tickets, pagos, comprobantes, clientes portal, etc.)
    /// conservando páginas, permisos, roles, usuarios admin, catálogo y configuración de empresa.
    /// </summary>
    public static class ProductionDataCleanup
    {
        public static void ClearTransactionalData(DataBaseService db, ILogger logger)
        {
            var isSqlite = DataBaseService.UseSqliteProvider;

            try
            {
                if (isSqlite)
                    db.Database.ExecuteSqlRaw("PRAGMA foreign_keys = OFF;");

                Execute(db, logger, isSqlite, BuildDeleteStatements(isSqlite));
                Execute(db, logger, isSqlite, BuildResetStatements(isSqlite));

                if (isSqlite)
                    db.Database.ExecuteSqlRaw("PRAGMA foreign_keys = ON;");

                logger.LogWarning(
                    "Datos transaccionales eliminados. Conservados: Pagina, Permiso, Rol, Usuario, Empresa, catálogo y contenido CMS.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "No se pudo limpiar datos transaccionales.");
                throw;
            }
        }

        /// <summary>Solo cotizaciones y pagos (desarrollo).</summary>
        public static void ClearCotizacionesOnly(DataBaseService db, ILogger logger)
        {
            if (!DataBaseService.UseSqliteProvider)
                return;

            try
            {
                db.Database.ExecuteSqlRaw("PRAGMA foreign_keys = OFF;");

                foreach (var sql in BuildCotizacionDeleteStatements(sqlite: true))
                    db.Database.ExecuteSqlRaw(sql);

                db.Database.ExecuteSqlRaw("PRAGMA foreign_keys = ON;");
                logger.LogWarning("DEV: Cotizaciones eliminadas. Clientes conservados.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "No se pudo limpiar cotizaciones.");
            }
        }

        private static IEnumerable<string> BuildDeleteStatements(bool sqlite)
        {
            yield return "DELETE FROM TicketVisto";
            yield return "DELETE FROM TicketMensaje";
            yield return "DELETE FROM TicketInterno";
            yield return "DELETE FROM ComprobanteDetalle";
            yield return "DELETE FROM ComprobanteElectronico";
            yield return "DELETE FROM PagoMercadoPago";
            yield return "DELETE FROM PagoVoucher";
            yield return "DELETE FROM CotizacionProducto";
            yield return "DELETE FROM CotizacionServicio";
            yield return "DELETE FROM CotizacionPaquete";
            yield return "DELETE FROM Cotizacion";
            yield return "DELETE FROM Cliente";
            yield return "DELETE FROM Contactanos";
            yield return sqlite
                ? "DELETE FROM LogAuditoria"
                : "DELETE FROM Auditoria.LogAuditoria";
            yield return sqlite
                ? "DELETE FROM LogErrores"
                : "DELETE FROM Auditoria.LogErrores";
            yield return "DELETE FROM Booking";
            yield return "DELETE FROM Customer";
            yield return "DELETE FROM [User]";
        }

        private static IEnumerable<string> BuildCotizacionDeleteStatements(bool sqlite)
        {
            yield return "DELETE FROM ComprobanteDetalle";
            yield return "DELETE FROM ComprobanteElectronico";
            yield return "DELETE FROM PagoMercadoPago";
            yield return "DELETE FROM PagoVoucher";
            yield return "DELETE FROM CotizacionProducto";
            yield return "DELETE FROM CotizacionServicio";
            yield return "DELETE FROM CotizacionPaquete";
            yield return "DELETE FROM Cotizacion";
        }

        private static IEnumerable<string> BuildResetStatements(bool sqlite)
        {
            yield return "UPDATE ComprobanteSerie SET UltimoCorrelativo = 0";

            if (sqlite)
            {
                yield return "DELETE FROM sqlite_sequence WHERE name IN ('Cotizacion','Cliente','TicketInterno','TicketMensaje','ComprobanteElectronico','PagoVoucher','PagoMercadoPago','Contactanos')";
            }
            else
            {
                yield return "DBCC CHECKIDENT ('Cotizacion', RESEED, 0)";
                yield return "DBCC CHECKIDENT ('Cliente', RESEED, 0)";
                yield return "DBCC CHECKIDENT ('TicketInterno', RESEED, 0)";
                yield return "DBCC CHECKIDENT ('TicketMensaje', RESEED, 0)";
                yield return "DBCC CHECKIDENT ('ComprobanteElectronico', RESEED, 0)";
                yield return "DBCC CHECKIDENT ('PagoVoucher', RESEED, 0)";
                yield return "DBCC CHECKIDENT ('PagoMercadoPago', RESEED, 0)";
                yield return "DBCC CHECKIDENT ('Contactanos', RESEED, 0)";
            }
        }

        private static void Execute(DataBaseService db, ILogger logger, bool sqlite, IEnumerable<string> statements)
        {
            foreach (var sql in statements)
            {
                try
                {
                    db.Database.ExecuteSqlRaw(sql);
                }
                catch (Exception ex)
                {
                    if (IsMissingObject(ex, sqlite))
                    {
                        logger.LogDebug("Omitido (tabla/columna inexistente): {Sql}", sql);
                        continue;
                    }

                    throw;
                }
            }
        }

        private static bool IsMissingObject(Exception ex, bool sqlite)
        {
            var message = ex.InnerException?.Message ?? ex.Message;

            if (sqlite)
                return message.Contains("no such table", StringComparison.OrdinalIgnoreCase);

            return message.Contains("Invalid object name", StringComparison.OrdinalIgnoreCase)
                || message.Contains("Cannot find the object", StringComparison.OrdinalIgnoreCase);
        }
    }
}
