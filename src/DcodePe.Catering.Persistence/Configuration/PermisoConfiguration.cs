using DcodePe.Catering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class PermisoConfiguration
    {
        public PermisoConfiguration(EntityTypeBuilder<PermisoEntity> entityBuilder) {

            entityBuilder.HasKey(e => e.PermisoID).HasName("PK__Permiso__96E0C70367B46FD4");

            entityBuilder.ToTable("Permiso");

            entityBuilder.Property(e => e.Estado).HasDefaultValue(true);
            entityBuilder.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entityBuilder.Property(e => e.FechaEliminacion).HasColumnType("datetime");
            entityBuilder.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entityBuilder.Property(e => e.PuedeCrear).HasDefaultValue(false);
            entityBuilder.Property(e => e.PuedeEditar).HasDefaultValue(false);
            entityBuilder.Property(e => e.PuedeEliminar).HasDefaultValue(false);
            entityBuilder.Property(e => e.PuedeVer).HasDefaultValue(false);
            entityBuilder.Property(e => e.UsuarioCreacion).HasMaxLength(100);
            entityBuilder.Property(e => e.UsuarioEliminacion).HasMaxLength(100);
            entityBuilder.Property(e => e.UsuarioModificacion).HasMaxLength(100);

            entityBuilder.HasOne(d => d.Pagina).WithMany(p => p.Permiso)
                .HasForeignKey(d => d.PaginaID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Permiso__PaginaI__45F365D3");

            entityBuilder.HasOne(d => d.Rol).WithMany(p => p.Permiso)
                .HasForeignKey(d => d.RolID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Permiso__RolID__44FF419A");
        }
    }
}
