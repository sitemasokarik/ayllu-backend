using DcodePe.Catering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class PaqueteServicioConfiguration
    {
        public PaqueteServicioConfiguration(EntityTypeBuilder<PaqueteServicioEntity> entityBuilder) {

            entityBuilder.HasKey(e => new { e.PaqueteID, e.ServicioID }).HasName("PK__PaqueteS__46C5C310A219C7EF");

            entityBuilder.ToTable("PaqueteServicio");

            entityBuilder.Property(e => e.Cantidad)
                .IsRequired()
                .HasDefaultValue(1);

            entityBuilder.Property(e => e.Estado).HasDefaultValue(true);
            entityBuilder.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entityBuilder.Property(e => e.FechaEliminacion).HasColumnType("datetime");
            entityBuilder.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entityBuilder.Property(e => e.UsuarioCreacion).HasMaxLength(100);
            entityBuilder.Property(e => e.UsuarioEliminacion).HasMaxLength(100);
            entityBuilder.Property(e => e.UsuarioModificacion).HasMaxLength(100);

            entityBuilder.HasOne(d => d.Paquete).WithMany(p => p.PaqueteServicio)
                .HasForeignKey(d => d.PaqueteID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaqueteSe__Paque__03F0984C");

            entityBuilder.HasOne(d => d.Servicio).WithMany(p => p.PaqueteServicio)
                .HasForeignKey(d => d.ServicioID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaqueteSe__Servi__04E4BC85");

        }

    }
}
