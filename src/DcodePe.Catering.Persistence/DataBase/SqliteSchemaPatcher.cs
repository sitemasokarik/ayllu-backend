using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Persistence.DataBase
{
    public static class SqliteSchemaPatcher
    {
        public static void Apply(DataBaseService db)
        {
            if (!DataBaseService.UseSqliteProvider)
                return;

            var patches = new[]
            {
                "ALTER TABLE Empresa ADD COLUMN BancoNombre TEXT",
                "ALTER TABLE Empresa ADD COLUMN NumeroCuenta TEXT",
                "ALTER TABLE Empresa ADD COLUMN Cci TEXT",
                "ALTER TABLE Empresa ADD COLUMN YapeNumero TEXT",
                "ALTER TABLE Empresa ADD COLUMN PlinNumero TEXT",
                "ALTER TABLE Empresa ADD COLUMN QrPagoUrl TEXT",
                "ALTER TABLE Empresa ADD COLUMN InstruccionesPago TEXT",
                "ALTER TABLE Empresa ADD COLUMN CuentasPagoJson TEXT",
                "ALTER TABLE Cotizacion ADD COLUMN OrigenCotizacion TEXT DEFAULT 'Admin'",
                @"CREATE TABLE IF NOT EXISTS PagoVoucher (
                    PagoVoucherID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CotizacionID INTEGER NOT NULL,
                    ClienteID INTEGER NOT NULL,
                    ArchivoUrl TEXT NOT NULL,
                    NombreArchivo TEXT,
                    Monto REAL NOT NULL DEFAULT 0,
                    EstadoPago TEXT NOT NULL DEFAULT 'Pendiente',
                    ObservacionCliente TEXT,
                    ObservacionAdmin TEXT,
                    UsuarioCreacion TEXT,
                    FechaCreacion TEXT,
                    UsuarioModificacion TEXT,
                    FechaModificacion TEXT,
                    Estado INTEGER DEFAULT 1,
                    FOREIGN KEY (CotizacionID) REFERENCES Cotizacion(CotizacionID),
                    FOREIGN KEY (ClienteID) REFERENCES Cliente(ClienteID)
                )",
                "CREATE INDEX IF NOT EXISTS IX_PagoVoucher_EstadoPago ON PagoVoucher(EstadoPago)",
                "CREATE INDEX IF NOT EXISTS IX_PagoVoucher_CotizacionID ON PagoVoucher(CotizacionID)",
                "ALTER TABLE Blog ADD COLUMN Resumen TEXT",
                "ALTER TABLE Blog ADD COLUMN MisionTitulo TEXT",
                "ALTER TABLE Blog ADD COLUMN MisionTexto TEXT",
                "ALTER TABLE Blog ADD COLUMN VisionTitulo TEXT",
                "ALTER TABLE Blog ADD COLUMN VisionTexto TEXT",
                "ALTER TABLE Blog ADD COLUMN LandingConfigJson TEXT",
                "ALTER TABLE Cotizacion ADD COLUMN BorradorJson TEXT",
                "ALTER TABLE Evento ADD COLUMN TarifasInvitadoJson TEXT",
                "ALTER TABLE Empresa ADD COLUMN MontoAdelantoReserva REAL DEFAULT 1000",
                "ALTER TABLE Empresa ADD COLUMN GeneraFactElect INTEGER DEFAULT 0",
                "ALTER TABLE Empresa ADD COLUMN Ubigeo TEXT",
                "ALTER TABLE Empresa ADD COLUMN RutaCertificadoServidor TEXT",
                "ALTER TABLE Empresa ADD COLUMN CertificadoFileName TEXT",
                "ALTER TABLE Empresa ADD COLUMN ClaveCertificado TEXT",
                "ALTER TABLE Empresa ADD COLUMN UsuarioSol TEXT",
                "ALTER TABLE Empresa ADD COLUMN ClaveSol TEXT",
                "ALTER TABLE Empresa ADD COLUMN SunatModo TEXT DEFAULT 'PRODUCCION'",
                "ALTER TABLE Empresa ADD COLUMN SunatWsProduccion TEXT",
                "ALTER TABLE ComprobanteElectronico ADD COLUMN PagoVoucherID INTEGER",
                "ALTER TABLE ComprobanteElectronico ADD COLUMN PagoMercadoPagoID INTEGER",
                "ALTER TABLE ComprobanteElectronico ADD COLUMN MontoAdelantoFacturado REAL DEFAULT 0",
                "ALTER TABLE ComprobanteElectronico ADD COLUMN SunatHashCpe TEXT",
                "ALTER TABLE ComprobanteElectronico ADD COLUMN RutaXml TEXT",
                "ALTER TABLE ComprobanteElectronico ADD COLUMN RutaCdr TEXT",
                "ALTER TABLE ComprobanteElectronico ADD COLUMN SunatCodigoError TEXT",
                @"CREATE TABLE IF NOT EXISTS PagoMercadoPago (
                    PagoMercadoPagoID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CotizacionID INTEGER NOT NULL,
                    ClienteID INTEGER NOT NULL,
                    Monto REAL NOT NULL DEFAULT 0,
                    MpPaymentId TEXT NOT NULL,
                    MpPreferenceId TEXT NOT NULL,
                    EstadoPago TEXT NOT NULL DEFAULT 'Pendiente',
                    MpStatusDetail TEXT,
                    FechaPago TEXT,
                    UsuarioCreacion TEXT,
                    FechaCreacion TEXT,
                    UsuarioModificacion TEXT,
                    FechaModificacion TEXT,
                    Estado INTEGER DEFAULT 1,
                    FOREIGN KEY (CotizacionID) REFERENCES Cotizacion(CotizacionID),
                    FOREIGN KEY (ClienteID) REFERENCES Cliente(ClienteID)
                )",
                "CREATE UNIQUE INDEX IF NOT EXISTS UQ_PagoMercadoPago_MpPaymentId ON PagoMercadoPago(MpPaymentId)",
                "CREATE INDEX IF NOT EXISTS IX_PagoMercadoPago_CotizacionID ON PagoMercadoPago(CotizacionID)",
                "CREATE INDEX IF NOT EXISTS IX_PagoMercadoPago_EstadoPago ON PagoMercadoPago(EstadoPago)",
                "ALTER TABLE Cotizacion ADD COLUMN FechaReservada TEXT",
                "ALTER TABLE PagoVoucher ADD COLUMN FechaReservadaElegida TEXT",
                "ALTER TABLE PagoMercadoPago ADD COLUMN FechaReservadaElegida TEXT",
                "ALTER TABLE Empresa ADD COLUMN ApiPeruDevToken TEXT",
                @"CREATE TABLE IF NOT EXISTS TicketVisto (
                    TicketVistoID INTEGER PRIMARY KEY AUTOINCREMENT,
                    TicketID INTEGER NOT NULL,
                    UsuarioID INTEGER NOT NULL,
                    FechaVisto TEXT NOT NULL,
                    FOREIGN KEY (TicketID) REFERENCES TicketInterno(TicketID)
                )",
                "CREATE UNIQUE INDEX IF NOT EXISTS IX_TicketVisto_Ticket_Usuario ON TicketVisto(TicketID, UsuarioID)",
                "ALTER TABLE Pagina ADD COLUMN GrupoMenu TEXT",
                "ALTER TABLE Pagina ADD COLUMN OrdenMenu INTEGER",
                "ALTER TABLE Cotizacion ADD COLUMN CreadoPorUsuarioID INTEGER",
                "ALTER TABLE Cotizacion ADD COLUMN CreadoPorNombre TEXT",
                "ALTER TABLE Cotizacion ADD COLUMN ResponsableUsuarioID INTEGER",
                "ALTER TABLE Cotizacion ADD COLUMN ResponsableNombre TEXT",
                "ALTER TABLE Cotizacion ADD COLUMN FechaAsignacion TEXT",
                "ALTER TABLE ServicioAdicional ADD COLUMN CantidadMinima INTEGER NOT NULL DEFAULT 1",
                "ALTER TABLE Local ADD COLUMN Garantia REAL NOT NULL DEFAULT 0"
            };

            foreach (var sql in patches)
            {
                try
                {
                    db.Database.ExecuteSqlRaw(sql);
                }
                catch
                {
                    // Columna o índice ya existente.
                }
            }

            BackfillFechaReservada(db);
            BackfillMenuGroups(db);
            EnsureFacturacionSubPages(db);
        }

        private static void BackfillFechaReservada(DataBaseService db)
        {
            var backfills = new[]
            {
                @"UPDATE Cotizacion
                  SET FechaReservada = (
                      SELECT pv.FechaReservadaElegida
                      FROM PagoVoucher pv
                      WHERE pv.CotizacionID = Cotizacion.CotizacionID
                        AND pv.Estado = 1
                        AND pv.EstadoPago = 'Aprobado'
                        AND pv.FechaReservadaElegida IS NOT NULL
                      ORDER BY pv.PagoVoucherID DESC
                      LIMIT 1
                  )
                  WHERE EstadoCotizacion = 'Evento'
                    AND FechaReservada IS NULL
                    AND EXISTS (
                      SELECT 1 FROM PagoVoucher pv
                      WHERE pv.CotizacionID = Cotizacion.CotizacionID
                        AND pv.Estado = 1
                        AND pv.EstadoPago = 'Aprobado'
                        AND pv.FechaReservadaElegida IS NOT NULL
                    )",
                @"UPDATE Cotizacion
                  SET FechaReservada = (
                      SELECT mp.FechaReservadaElegida
                      FROM PagoMercadoPago mp
                      WHERE mp.CotizacionID = Cotizacion.CotizacionID
                        AND mp.Estado = 1
                        AND mp.EstadoPago = 'Aprobado'
                        AND mp.FechaReservadaElegida IS NOT NULL
                      ORDER BY mp.PagoMercadoPagoID DESC
                      LIMIT 1
                  )
                  WHERE EstadoCotizacion = 'Evento'
                    AND FechaReservada IS NULL
                    AND EXISTS (
                      SELECT 1 FROM PagoMercadoPago mp
                      WHERE mp.CotizacionID = Cotizacion.CotizacionID
                        AND mp.Estado = 1
                        AND mp.EstadoPago = 'Aprobado'
                        AND mp.FechaReservadaElegida IS NOT NULL
                    )"
            };

            foreach (var sql in backfills)
            {
                try
                {
                    db.Database.ExecuteSqlRaw(sql);
                }
                catch
                {
                    // Ignorar si la tabla aún no existe en este entorno.
                }
            }
        }

        private static void BackfillMenuGroups(DataBaseService db)
        {
            var updates = new[]
            {
                "UPDATE Pagina SET GrupoMenu = NULL, OrdenMenu = 1 WHERE PaginaID = 1",
                "UPDATE Pagina SET GrupoMenu = 'Ventas y cotizaciones', OrdenMenu = 1 WHERE PaginaID = 2",
                "UPDATE Pagina SET GrupoMenu = 'Ventas y cotizaciones', OrdenMenu = 2 WHERE PaginaID = 3",
                "UPDATE Pagina SET GrupoMenu = 'Ventas y cotizaciones', OrdenMenu = 3 WHERE PaginaID = 4",
                "UPDATE Pagina SET GrupoMenu = 'Eventos y locales', OrdenMenu = 1 WHERE PaginaID = 5",
                "UPDATE Pagina SET GrupoMenu = 'Eventos y locales', OrdenMenu = 2 WHERE PaginaID = 6",
                "UPDATE Pagina SET GrupoMenu = 'Eventos y locales', OrdenMenu = 3 WHERE PaginaID = 13",
                "UPDATE Pagina SET GrupoMenu = 'Usuarios y clientes', OrdenMenu = 1 WHERE PaginaID = 7",
                "UPDATE Pagina SET GrupoMenu = 'Usuarios y clientes', OrdenMenu = 2 WHERE PaginaID = 8",
                "UPDATE Pagina SET GrupoMenu = 'Catálogo', OrdenMenu = 1 WHERE PaginaID = 9",
                "UPDATE Pagina SET GrupoMenu = 'Catálogo', OrdenMenu = 2 WHERE PaginaID = 10",
                "UPDATE Pagina SET GrupoMenu = 'Seguridad y accesos', OrdenMenu = 1 WHERE PaginaID = 11",
                "UPDATE Pagina SET GrupoMenu = 'Seguridad y accesos', OrdenMenu = 2 WHERE PaginaID = 12",
                "UPDATE Pagina SET GrupoMenu = 'Seguridad y accesos', OrdenMenu = 3 WHERE PaginaID = 14",
                "UPDATE Pagina SET GrupoMenu = NULL, OrdenMenu = 80 WHERE PaginaID = 15",
                "UPDATE Pagina SET GrupoMenu = NULL, OrdenMenu = 90 WHERE PaginaID = 16",
                "UPDATE Pagina SET GrupoMenu = NULL, OrdenMenu = 0 WHERE PaginaID = 17",
                "UPDATE Pagina SET GrupoMenu = NULL, OrdenMenu = 60 WHERE PaginaID = 18",
                "UPDATE Pagina SET GrupoMenu = 'Facturación y pagos', OrdenMenu = 4 WHERE PaginaID = 19",
                "UPDATE Pagina SET GrupoMenu = 'Facturación y pagos', OrdenMenu = 1 WHERE PaginaID = 20",
                "UPDATE Pagina SET GrupoMenu = 'Facturación y pagos', OrdenMenu = 2 WHERE PaginaID = 21",
                "UPDATE Pagina SET GrupoMenu = 'Facturación y pagos', OrdenMenu = 3 WHERE PaginaID = 22"
            };

            foreach (var sql in updates)
            {
                try { db.Database.ExecuteSqlRaw(sql); } catch { }
            }
        }

        private static void EnsureFacturacionSubPages(DataBaseService db)
        {
            var inserts = new[]
            {
                @"INSERT INTO Pagina (PaginaID, Nombre, Descripcion, Url, Icono, UsuarioCreacion, FechaCreacion, Estado, GrupoMenu, OrdenMenu)
                  SELECT 20, 'Emitir Boleta', 'Emitir Boleta', 'facturacion/emitir-boleta', 'bill-line', 'patch', datetime('now'), 1, 'Facturación y pagos', 1
                  WHERE NOT EXISTS (SELECT 1 FROM Pagina WHERE PaginaID = 20)",
                @"INSERT INTO Pagina (PaginaID, Nombre, Descripcion, Url, Icono, UsuarioCreacion, FechaCreacion, Estado, GrupoMenu, OrdenMenu)
                  SELECT 21, 'Emitir Factura', 'Emitir Factura', 'facturacion/emitir-factura', 'bill-line', 'patch', datetime('now'), 1, 'Facturación y pagos', 2
                  WHERE NOT EXISTS (SELECT 1 FROM Pagina WHERE PaginaID = 21)",
                @"INSERT INTO Pagina (PaginaID, Nombre, Descripcion, Url, Icono, UsuarioCreacion, FechaCreacion, Estado, GrupoMenu, OrdenMenu)
                  SELECT 22, 'Comprobantes', 'Comprobantes electrónicos', 'facturacion/comprobantes', 'file-list-line', 'patch', datetime('now'), 1, 'Facturación y pagos', 3
                  WHERE NOT EXISTS (SELECT 1 FROM Pagina WHERE PaginaID = 22)",
                @"INSERT INTO Permiso (RolID, PaginaID, PuedeVer, PuedeCrear, PuedeEditar, PuedeEliminar, UsuarioCreacion, FechaCreacion, Estado)
                  SELECT 1, 20, 1, 1, 1, 1, 'patch', datetime('now'), 1
                  WHERE NOT EXISTS (SELECT 1 FROM Permiso WHERE RolID = 1 AND PaginaID = 20)",
                @"INSERT INTO Permiso (RolID, PaginaID, PuedeVer, PuedeCrear, PuedeEditar, PuedeEliminar, UsuarioCreacion, FechaCreacion, Estado)
                  SELECT 1, 21, 1, 1, 1, 1, 'patch', datetime('now'), 1
                  WHERE NOT EXISTS (SELECT 1 FROM Permiso WHERE RolID = 1 AND PaginaID = 21)",
                @"INSERT INTO Permiso (RolID, PaginaID, PuedeVer, PuedeCrear, PuedeEditar, PuedeEliminar, UsuarioCreacion, FechaCreacion, Estado)
                  SELECT 1, 22, 1, 1, 1, 1, 'patch', datetime('now'), 1
                  WHERE NOT EXISTS (SELECT 1 FROM Permiso WHERE RolID = 1 AND PaginaID = 22)"
            };

            foreach (var sql in inserts)
            {
                try { db.Database.ExecuteSqlRaw(sql); } catch { }
            }
        }
    }
}
