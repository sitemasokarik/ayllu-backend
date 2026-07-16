using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Persistence.DataBase
{
    internal static class SqliteCompatibility
    {
        public static void Apply(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!string.IsNullOrEmpty(entityType.GetSchema()))
                    entityType.SetSchema(null);

                foreach (var property in entityType.GetProperties())
                {
                    var columnType = property.GetColumnType();
                    if (!string.IsNullOrEmpty(columnType))
                        property.SetColumnType(null);

                    var defaultSql = property.GetDefaultValueSql();
                    if (!string.IsNullOrEmpty(defaultSql) &&
                        defaultSql.Contains("getdate", StringComparison.OrdinalIgnoreCase))
                    {
                        property.SetDefaultValueSql("CURRENT_TIMESTAMP");
                    }
                }
            }
        }
    }
}
