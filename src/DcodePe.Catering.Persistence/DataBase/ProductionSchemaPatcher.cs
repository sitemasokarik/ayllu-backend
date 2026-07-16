using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DcodePe.Catering.Persistence.DataBase
{
    /// <summary>
    /// Aplica en SQL Server (producción) columnas/tablas/páginas que en dev se parchean con SqliteSchemaPatcher.
    /// </summary>
    public static class ProductionSchemaPatcher
    {
        public static void Apply(DataBaseService db, ILogger logger)
        {
            if (DataBaseService.UseSqliteProvider)
                return;

            ApplySqlServerSchema(db, logger);
            BackfillMenuGroups(db);
            EnsureFacturacionSubPages(db);
        }

        private static void ApplySqlServerSchema(DataBaseService db, ILogger logger)
        {
            var patches = new[]
            {
                @"IF COL_LENGTH('Blog', 'LandingConfigJson') IS NULL
                  ALTER TABLE Blog ADD LandingConfigJson NVARCHAR(MAX) NULL",
                @"IF COL_LENGTH('Cliente', 'EsPortalActivo') IS NULL
                  ALTER TABLE Cliente ADD EsPortalActivo BIT NOT NULL CONSTRAINT DF_Cliente_EsPortalActivo DEFAULT 0",
                @"IF COL_LENGTH('Cliente', 'PasswordHash') IS NULL
                  ALTER TABLE Cliente ADD PasswordHash NVARCHAR(255) NULL",
                @"IF COL_LENGTH('Cliente', 'UserNamePortal') IS NULL
                  ALTER TABLE Cliente ADD UserNamePortal NVARCHAR(100) NULL",
                @"IF COL_LENGTH('Cliente', 'UserNamePortal') IS NOT NULL
                  AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_Cliente_UserNamePortal')
                  CREATE UNIQUE INDEX UQ_Cliente_UserNamePortal ON Cliente(UserNamePortal) WHERE UserNamePortal IS NOT NULL",
                @"IF COL_LENGTH('Cotizacion', 'OrigenCotizacion') IS NULL
                  ALTER TABLE Cotizacion ADD OrigenCotizacion NVARCHAR(50) NOT NULL CONSTRAINT DF_Cotizacion_Origen DEFAULT 'Admin'",
                @"IF COL_LENGTH('Cotizacion', 'FechaReservada') IS NULL
                  ALTER TABLE Cotizacion ADD FechaReservada DATETIME NULL",
                @"IF COL_LENGTH('Cotizacion', 'BorradorJson') IS NULL
                  ALTER TABLE Cotizacion ADD BorradorJson NVARCHAR(MAX) NULL",
                @"IF COL_LENGTH('Cotizacion', 'CreadoPorUsuarioID') IS NULL
                  ALTER TABLE Cotizacion ADD CreadoPorUsuarioID INT NULL",
                @"IF COL_LENGTH('Cotizacion', 'CreadoPorNombre') IS NULL
                  ALTER TABLE Cotizacion ADD CreadoPorNombre NVARCHAR(100) NULL",
                @"IF COL_LENGTH('Cotizacion', 'ResponsableUsuarioID') IS NULL
                  ALTER TABLE Cotizacion ADD ResponsableUsuarioID INT NULL",
                @"IF COL_LENGTH('Cotizacion', 'ResponsableNombre') IS NULL
                  ALTER TABLE Cotizacion ADD ResponsableNombre NVARCHAR(100) NULL",
                @"IF COL_LENGTH('Cotizacion', 'FechaAsignacion') IS NULL
                  ALTER TABLE Cotizacion ADD FechaAsignacion DATETIME NULL",
                @"IF COL_LENGTH('Pagina', 'GrupoMenu') IS NULL
                  ALTER TABLE Pagina ADD GrupoMenu NVARCHAR(100) NULL",
                @"IF COL_LENGTH('Pagina', 'OrdenMenu') IS NULL
                  ALTER TABLE Pagina ADD OrdenMenu INT NULL",
                @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TicketVisto')
                  CREATE TABLE TicketVisto (
                    TicketVistoID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                    TicketID INT NOT NULL,
                    UsuarioID INT NOT NULL,
                    FechaVisto DATETIME2 NOT NULL,
                    CONSTRAINT FK_TicketVisto_TicketInterno FOREIGN KEY (TicketID) REFERENCES TicketInterno(TicketID)
                  )",
                @"IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TicketVisto_Ticket_Usuario')
                  CREATE UNIQUE INDEX IX_TicketVisto_Ticket_Usuario ON TicketVisto(TicketID, UsuarioID)",
                @"IF COL_LENGTH('ServicioAdicional', 'CantidadMinima') IS NULL
                  ALTER TABLE ServicioAdicional ADD CantidadMinima INT NOT NULL CONSTRAINT DF_ServicioAdicional_CantidadMinima DEFAULT 1",
                @"IF COL_LENGTH('Local', 'Garantia') IS NULL
                  ALTER TABLE Local ADD Garantia DECIMAL(10,2) NOT NULL CONSTRAINT DF_Local_Garantia DEFAULT 0"
            };

            foreach (var sql in patches)
            {
                try
                {
                    db.Database.ExecuteSqlRaw(sql);
                }
                catch (Exception ex)
                {
                    logger.LogDebug(ex, "Parche de esquema omitido o ya aplicado.");
                }
            }
        }

        private static void BackfillMenuGroups(DataBaseService db)
        {
            var updates = new[]
            {
                "UPDATE Pagina SET GrupoMenu = NULL, OrdenMenu = 1 WHERE PaginaID = 1",
                "UPDATE Pagina SET GrupoMenu = N'Ventas y cotizaciones', OrdenMenu = 1 WHERE PaginaID = 2",
                "UPDATE Pagina SET GrupoMenu = N'Ventas y cotizaciones', OrdenMenu = 2 WHERE PaginaID = 3",
                "UPDATE Pagina SET GrupoMenu = N'Ventas y cotizaciones', OrdenMenu = 3 WHERE PaginaID = 4",
                "UPDATE Pagina SET GrupoMenu = N'Eventos y locales', OrdenMenu = 1 WHERE PaginaID = 5",
                "UPDATE Pagina SET GrupoMenu = N'Eventos y locales', OrdenMenu = 2 WHERE PaginaID = 6",
                "UPDATE Pagina SET GrupoMenu = N'Eventos y locales', OrdenMenu = 3 WHERE PaginaID = 13",
                "UPDATE Pagina SET GrupoMenu = N'Usuarios y clientes', OrdenMenu = 1 WHERE PaginaID = 7",
                "UPDATE Pagina SET GrupoMenu = N'Usuarios y clientes', OrdenMenu = 2 WHERE PaginaID = 8",
                "UPDATE Pagina SET GrupoMenu = N'Catálogo', OrdenMenu = 1 WHERE PaginaID = 9",
                "UPDATE Pagina SET GrupoMenu = N'Catálogo', OrdenMenu = 2 WHERE PaginaID = 10",
                "UPDATE Pagina SET GrupoMenu = N'Seguridad y accesos', OrdenMenu = 1 WHERE PaginaID = 11",
                "UPDATE Pagina SET GrupoMenu = N'Seguridad y accesos', OrdenMenu = 2 WHERE PaginaID = 12",
                "UPDATE Pagina SET GrupoMenu = N'Seguridad y accesos', OrdenMenu = 3 WHERE PaginaID = 14",
                "UPDATE Pagina SET GrupoMenu = NULL, OrdenMenu = 80 WHERE PaginaID = 15",
                "UPDATE Pagina SET GrupoMenu = NULL, OrdenMenu = 90 WHERE PaginaID = 16",
                "UPDATE Pagina SET GrupoMenu = NULL, OrdenMenu = 0 WHERE PaginaID = 17",
                "UPDATE Pagina SET GrupoMenu = NULL, OrdenMenu = 60 WHERE PaginaID = 18",
                "UPDATE Pagina SET GrupoMenu = N'Facturación y pagos', OrdenMenu = 4 WHERE PaginaID = 19",
                "UPDATE Pagina SET GrupoMenu = N'Facturación y pagos', OrdenMenu = 1 WHERE PaginaID = 20",
                "UPDATE Pagina SET GrupoMenu = N'Facturación y pagos', OrdenMenu = 2 WHERE PaginaID = 21",
                "UPDATE Pagina SET GrupoMenu = N'Facturación y pagos', OrdenMenu = 3 WHERE PaginaID = 22"
            };

            foreach (var sql in updates)
            {
                try { db.Database.ExecuteSqlRaw(sql); } catch { }
            }
        }

        private static void EnsureFacturacionSubPages(DataBaseService db)
        {
            var statements = new[]
            {
                "SET IDENTITY_INSERT Pagina ON",
                @"IF NOT EXISTS (SELECT 1 FROM Pagina WHERE PaginaID = 20)
                  INSERT INTO Pagina (PaginaID, Nombre, Descripcion, Url, Icono, UsuarioCreacion, FechaCreacion, Estado, GrupoMenu, OrdenMenu)
                  VALUES (20, N'Emitir Boleta', N'Emitir Boleta', N'facturacion/emitir-boleta', N'bill-line', N'patch', GETDATE(), 1, N'Facturación y pagos', 1)",
                @"IF NOT EXISTS (SELECT 1 FROM Pagina WHERE PaginaID = 21)
                  INSERT INTO Pagina (PaginaID, Nombre, Descripcion, Url, Icono, UsuarioCreacion, FechaCreacion, Estado, GrupoMenu, OrdenMenu)
                  VALUES (21, N'Emitir Factura', N'Emitir Factura', N'facturacion/emitir-factura', N'bill-line', N'patch', GETDATE(), 1, N'Facturación y pagos', 2)",
                @"IF NOT EXISTS (SELECT 1 FROM Pagina WHERE PaginaID = 22)
                  INSERT INTO Pagina (PaginaID, Nombre, Descripcion, Url, Icono, UsuarioCreacion, FechaCreacion, Estado, GrupoMenu, OrdenMenu)
                  VALUES (22, N'Comprobantes', N'Comprobantes electrónicos', N'facturacion/comprobantes', N'file-list-line', N'patch', GETDATE(), 1, N'Facturación y pagos', 3)",
                "SET IDENTITY_INSERT Pagina OFF",
                @"IF NOT EXISTS (SELECT 1 FROM Permiso WHERE RolID = 1 AND PaginaID = 20)
                  INSERT INTO Permiso (RolID, PaginaID, PuedeVer, PuedeCrear, PuedeEditar, PuedeEliminar, UsuarioCreacion, FechaCreacion, Estado)
                  VALUES (1, 20, 1, 1, 1, 1, N'patch', GETDATE(), 1)",
                @"IF NOT EXISTS (SELECT 1 FROM Permiso WHERE RolID = 1 AND PaginaID = 21)
                  INSERT INTO Permiso (RolID, PaginaID, PuedeVer, PuedeCrear, PuedeEditar, PuedeEliminar, UsuarioCreacion, FechaCreacion, Estado)
                  VALUES (1, 21, 1, 1, 1, 1, N'patch', GETDATE(), 1)",
                @"IF NOT EXISTS (SELECT 1 FROM Permiso WHERE RolID = 1 AND PaginaID = 22)
                  INSERT INTO Permiso (RolID, PaginaID, PuedeVer, PuedeCrear, PuedeEditar, PuedeEliminar, UsuarioCreacion, FechaCreacion, Estado)
                  VALUES (1, 22, 1, 1, 1, 1, N'patch', GETDATE(), 1)"
            };

            foreach (var sql in statements)
            {
                try { db.Database.ExecuteSqlRaw(sql); } catch { }
            }
        }
    }
}
