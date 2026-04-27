using DcodePe.Catering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Persistence.Configuration
{
    public  class RolConfiguration
    {
        public RolConfiguration(EntityTypeBuilder<RolEntity> entityBuilder) {

            entityBuilder.HasKey(e => e.RolID).HasName("PK__Rol__F92302D107BDE9CD");

            entityBuilder.ToTable("Rol");

            entityBuilder.Property(e => e.Descripcion).HasMaxLength(255);
            entityBuilder.Property(e => e.Estado).HasDefaultValue(true);
            entityBuilder.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entityBuilder.Property(e => e.FechaEliminacion).HasColumnType("datetime");
            entityBuilder.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entityBuilder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);
            entityBuilder.Property(e => e.UsuarioCreacion).HasMaxLength(100);
            entityBuilder.Property(e => e.UsuarioEliminacion).HasMaxLength(100);
            entityBuilder.Property(e => e.UsuarioModificacion).HasMaxLength(100);

        }
    }
}
