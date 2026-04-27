using DcodePe.Catering.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class CotizacionPaqueteConfiguration
    {
        public CotizacionPaqueteConfiguration(EntityTypeBuilder<CotizacionPaqueteEntity> entityBuilder)
        {
            entityBuilder.HasKey(e => new { e.CotizacionID, e.PaqueteID })
                .HasName("PK__CotizacionPaquete");

            entityBuilder.ToTable("CotizacionPaquete");

            entityBuilder.Property(e => e.Cantidad)
                .IsRequired()
                .HasDefaultValue(1);

            entityBuilder.Property(e => e.Estado)
                .HasDefaultValue(true);

            entityBuilder.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entityBuilder.Property(e => e.FechaEliminacion)
                .HasColumnType("datetime");

            entityBuilder.Property(e => e.FechaModificacion)
                .HasColumnType("datetime");

            entityBuilder.Property(e => e.UsuarioCreacion)
                .HasMaxLength(100);

            entityBuilder.Property(e => e.UsuarioEliminacion)
                .HasMaxLength(100);

            entityBuilder.Property(e => e.UsuarioModificacion)
                .HasMaxLength(100);

            entityBuilder.HasOne(d => d.Cotizacion)
                .WithMany(p => p.CotizacionPaquete)
                .HasForeignKey(d => d.CotizacionID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CotizacionPaquete__Cotizacion");

            entityBuilder.HasOne(d => d.Paquete)
                .WithMany(p => p.CotizacionPaquete)
                .HasForeignKey(d => d.PaqueteID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CotizacionPaquete__Paquete");
        }
    }
}
