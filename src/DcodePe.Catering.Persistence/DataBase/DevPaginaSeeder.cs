using DcodePe.Catering.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Persistence.DataBase
{
    public static class DevPaginaSeeder
    {
        private static readonly (int Id, string Nombre, string Icono, string Url)[] ExtraPages =
        [
            (17, "Facturación", "bill-line", "facturacion/comprobantes"),
            (18, "Tickets internos", "customer-service-2-line", "tickets"),
            (19, "Vouchers de pago", "money-dollar-circle-line", "pagos-vouchers"),
        ];

        public static void EnsurePages(DataBaseService db)
        {
            if (!DataBaseService.UseSqliteProvider)
                return;

            var changed = false;

            foreach (var p in ExtraPages)
            {
                if (db.Pagina.Any(x => x.PaginaID == p.Id))
                    continue;

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
                changed = true;
            }

            if (changed)
                db.SaveChanges();

            foreach (var p in ExtraPages)
            {
                if (db.Permiso.Any(x => x.PaginaID == p.Id && x.RolID == 1))
                    continue;

                db.Permiso.Add(new PermisoEntity
                {
                    RolID = 1,
                    PaginaID = p.Id,
                    PuedeVer = true,
                    PuedeCrear = true,
                    PuedeEditar = true,
                    PuedeEliminar = true,
                    UsuarioCreacion = "seed",
                    FechaCreacion = DateTime.UtcNow,
                    Estado = true
                });
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                // Permisos/páginas ya sembrados por dump o arranque previo.
            }
        }
    }
}
