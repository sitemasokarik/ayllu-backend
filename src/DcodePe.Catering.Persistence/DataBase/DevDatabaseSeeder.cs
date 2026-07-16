using DcodePe.Catering.Domain.Entities;
using DcodePe.Catering.Domain.Entities.Facturacion;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Persistence.DataBase
{
    /// <summary>
    /// Datos mínimos para desarrollo local (SQLite), tomados del dump de producción DB/db.sql.
    /// Las contraseñas son los mismos hashes BCrypt de producción: usa el mismo user/password que en el servidor.
    /// </summary>
    public static class DevDatabaseSeeder
    {
        public static void Seed(DataBaseService db)
        {
            SeedSecurity(db);
            SeedComprobanteSeries(db);
        }

        private static void SeedSecurity(DataBaseService db)
        {
            if (db.Usuario.Any())
                return;

            if (!db.Rol.Any())
            {
                db.Rol.AddRange(
                    new RolEntity
                    {
                        RolID = 1,
                        Nombre = "Administrador",
                        Descripcion = "Administrador",
                        UsuarioCreacion = "seed",
                        FechaCreacion = DateTime.UtcNow,
                        Estado = true
                    },
                    new RolEntity
                    {
                        RolID = 2,
                        Nombre = "Ejecutivo de Ventas",
                        Descripcion = "Ejecutivo de Ventas",
                        UsuarioCreacion = "seed",
                        FechaCreacion = DateTime.UtcNow,
                        Estado = true
                    },
                    new RolEntity
                    {
                        RolID = 3,
                        Nombre = "Ejecutivo de Productos",
                        Descripcion = "Ejecutivo de Productos",
                        UsuarioCreacion = "seed",
                        FechaCreacion = DateTime.UtcNow,
                        Estado = true
                    }
                );
                db.SaveChanges();
            }

            if (!db.Pagina.Any())
            {
                var paginas = new (int Id, string Nombre, string Icono, string Url)[]
                {
                    (1, "Inicio", "home-line", "home"),
                    (2, "Presupuestador", "rocket-line", "presupuestador"),
                    (3, "Listado Cotizaciones", "article-line", "table-cotizaciones"),
                    (4, "Contáctanos", "contacts-book-2-line", "table-forms"),
                    (5, "Locales", "store-3-line", "locales"),
                    (6, "Servicios", "dashboard-horizontal-line", "services"),
                    (7, "Usuarios", "shield-user-line", "users"),
                    (8, "Clientes", "user-line", "clients"),
                    (9, "Categorías", "layout-3-fill", "categorys"),
                    (10, "Productos", "layout-grid-2-line", "products"),
                    (11, "Roles", "share-line", "roles"),
                    (12, "Permisos", "share-line", "permisos"),
                    (13, "Eventos", "menu-fold-4-line", "events"),
                    (14, "Paginas", "page-separator", "pages"),
                    (15, "Landing Page", "dice-4-line", "landing-page"),
                    (16, "Empresa", "list-settings-line", "company"),
                    (17, "Facturación", "bill-line", "facturacion/comprobantes"),
                    (18, "Tickets internos", "customer-service-2-line", "tickets"),
                    (19, "Vouchers de pago", "money-dollar-circle-line", "pagos-vouchers")
                };

                foreach (var p in paginas)
                {
                    db.Pagina.Add(new PaginaEntity
                    {
                        PaginaID = p.Id,
                        Nombre = p.Nombre,
                        Descripcion = p.Nombre,
                        Icono = p.Icono,
                        Url = p.Url,
                        UsuarioCreacion = "seed",
                        FechaCreacion = DateTime.UtcNow,
                        Estado = true
                    });
                }

                db.SaveChanges();
            }

            if (!db.Permiso.Any())
            {
                for (var paginaId = 1; paginaId <= 19; paginaId++)
                {
                    db.Permiso.Add(new PermisoEntity
                    {
                        RolID = 1,
                        PaginaID = paginaId,
                        PuedeVer = true,
                        PuedeCrear = true,
                        PuedeEditar = true,
                        PuedeEliminar = true,
                        UsuarioCreacion = "seed",
                        FechaCreacion = DateTime.UtcNow,
                        Estado = true
                    });
                }

                db.SaveChanges();
            }

            db.Usuario.AddRange(
                new UsuarioEntity
                {
                    UsuarioID = 1,
                    Nombre = "Administrador",
                    UserName = "admin",
                    Email = "eventosyfiestasayllu@gmail.com",
                    Password = "$2a$12$Xi/57fHBMqD/zR/VN5tfTu3DFhWsnbXnwBRA6wHATuBBPg2o.TiSa",
                    RolID = 1,
                    UsuarioCreacion = "seed",
                    FechaCreacion = DateTime.UtcNow,
                    Estado = true
                },
                new UsuarioEntity
                {
                    UsuarioID = 4,
                    Nombre = "Cesar Huamani",
                    UserName = "chuamani",
                    Email = "cesarhuamanicastro@gmail.com",
                    Password = "$2a$12$spF97zAtW6lV7fzb2P/XqO5Xst8lu.lfpjSHI.ND6XyCj8.90sYgm",
                    RolID = 1,
                    UsuarioCreacion = "seed",
                    FechaCreacion = DateTime.UtcNow,
                    Estado = true
                }
            );

            db.SaveChanges();
        }

        private static void SeedComprobanteSeries(DataBaseService db)
        {
            if (db.ComprobanteSerie.Any())
                return;

            db.ComprobanteSerie.AddRange(
                new ComprobanteSerieEntity { Tipo = "boleta", Serie = "B001", UltimoCorrelativo = 0 },
                new ComprobanteSerieEntity { Tipo = "factura", Serie = "F001", UltimoCorrelativo = 0 }
            );

            db.SaveChanges();
        }
    }
}
