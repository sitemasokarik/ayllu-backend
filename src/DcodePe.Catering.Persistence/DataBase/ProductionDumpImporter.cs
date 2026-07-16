using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DcodePe.Catering.Persistence.DataBase
{
    public static class ProductionDumpImporter
    {
        private static readonly string[] ImportOrder =
        [
            "Rol",
            "Pagina",
            "Permiso",
            "Usuario",
            "Categoria",
            "Empresa",
            "Blog",
            "Local",
            "Evento",
            "Cliente",
            "Producto",
            "ServicioAdicional",
            "Cotizacion",
            "CotizacionProducto",
            "CotizacionServicio",
            "Contactanos"
        ];

        public static void ImportIfNeeded(DataBaseService db, string dumpPath, ILogger logger)
        {
            if (db.Evento.Any() && db.Local.Any() && db.Rol.Any())
            {
                DevDatabaseSeeder.Seed(db);
                return;
            }

            if (!File.Exists(dumpPath))
            {
                logger.LogWarning("No se encontró el dump en {Path}. Usando seed mínimo.", dumpPath);
                DevDatabaseSeeder.Seed(db);
                return;
            }

            logger.LogInformation("Importando datos de producción desde {Path}...", dumpPath);
            var content = File.ReadAllText(dumpPath);

            db.Database.ExecuteSqlRaw("PRAGMA foreign_keys = OFF;");

            var imported = 0;
            foreach (var table in ImportOrder)
            {
                foreach (var insert in ExtractInserts(content, table))
                {
                    var sql = ConvertToSqlite(insert);
                    try
                    {
                        db.Database.ExecuteSqlRaw(sql);
                        imported++;
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "Error importando {Table}", table);
                    }
                }
            }

            db.Database.ExecuteSqlRaw("PRAGMA foreign_keys = ON;");
            NormalizeBooleanColumns(db);
            DevDatabaseSeeder.Seed(db);

            logger.LogInformation("Importación completada: {Count} sentencias ejecutadas.", imported);
        }

        public static string ResolveDumpPath(string contentRootPath, string? configuredPath)
        {
            if (!string.IsNullOrWhiteSpace(configuredPath) && File.Exists(configuredPath))
                return Path.GetFullPath(configuredPath);

            var candidates = new[]
            {
                Path.GetFullPath(Path.Combine(contentRootPath, "../../../DB/db.sql")),
                Path.GetFullPath(Path.Combine(contentRootPath, "../../../../DB/db.sql")),
                Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "DB/db.sql")),
            };

            return candidates.FirstOrDefault(File.Exists) ?? candidates[0];
        }

        private static IEnumerable<string> ExtractInserts(string content, string table)
        {
            var pattern = $@"INSERT INTO \[dbo\]\.\[{table}\][\s\S]*?;";
            return Regex.Matches(content, pattern).Select(m => m.Value);
        }

        private static string ConvertToSqlite(string sql)
        {
            sql = sql.Replace("[dbo].", string.Empty, StringComparison.OrdinalIgnoreCase);
            sql = sql.Replace("[", string.Empty).Replace("]", string.Empty);
            sql = Regex.Replace(sql, @"N'", "'");
            sql = Regex.Replace(sql, @",\s*\.(\d)", ",0.$1");
            sql = Regex.Replace(sql, @"\t\.(\d)", "\t0.$1");
            sql = Regex.Replace(sql, @",\s*\.(\s|,|\))", ",0$1");
            sql = Regex.Replace(sql, @"\t\.(\s|,|\))", "\t0$1");
            sql = EscapeBracesForEfRawSql(sql);
            return sql;
        }

        /// <summary>
        /// EF ExecuteSqlRaw interpreta { } como placeholders de formato.
        /// </summary>
        private static string EscapeBracesForEfRawSql(string sql)
        {
            return sql.Replace("{", "{{", StringComparison.Ordinal).Replace("}", "}}", StringComparison.Ordinal);
        }

        private static void NormalizeBooleanColumns(DataBaseService db)
        {
            var statements = new[]
            {
                "UPDATE Rol SET Estado = CASE WHEN CAST(Estado AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END",
                "UPDATE Pagina SET Estado = CASE WHEN CAST(Estado AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END",
                "UPDATE Permiso SET PuedeVer = CASE WHEN CAST(PuedeVer AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END, PuedeCrear = CASE WHEN CAST(PuedeCrear AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END, PuedeEditar = CASE WHEN CAST(PuedeEditar AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END, PuedeEliminar = CASE WHEN CAST(PuedeEliminar AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END, Estado = CASE WHEN CAST(Estado AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END",
                "UPDATE Usuario SET Estado = CASE WHEN CAST(Estado AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END",
                "UPDATE Categoria SET Estado = CASE WHEN CAST(Estado AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END",
                "UPDATE Empresa SET Estado = CASE WHEN CAST(Estado AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END",
                "UPDATE Blog SET Estado = CASE WHEN CAST(Estado AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END",
                "UPDATE Local SET Estado = CASE WHEN CAST(Estado AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END",
                "UPDATE Evento SET Estado = CASE WHEN CAST(Estado AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END",
                "UPDATE Cliente SET Estado = CASE WHEN CAST(Estado AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END",
                "UPDATE Producto SET Estado = CASE WHEN CAST(Estado AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END",
                "UPDATE ServicioAdicional SET Estado = CASE WHEN CAST(Estado AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END",
                "UPDATE Cotizacion SET Estado = CASE WHEN CAST(Estado AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END",
                "UPDATE CotizacionProducto SET Estado = CASE WHEN CAST(Estado AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END",
                "UPDATE CotizacionServicio SET Estado = CASE WHEN CAST(Estado AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END",
                "UPDATE Contactanos SET Estado = CASE WHEN CAST(Estado AS TEXT) IN ('1','true','True') THEN 1 ELSE 0 END",
            };

            foreach (var statement in statements)
            {
                try
                {
                    db.Database.ExecuteSqlRaw(statement);
                }
                catch
                {
                    // Tabla puede no existir aún en algunos entornos.
                }
            }
        }
    }
}
