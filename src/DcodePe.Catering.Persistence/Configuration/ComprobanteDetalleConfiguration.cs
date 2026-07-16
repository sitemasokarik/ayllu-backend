using DcodePe.Catering.Domain.Entities.Facturacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class ComprobanteDetalleConfiguration : IEntityTypeConfiguration<ComprobanteDetalleEntity>
    {
        public void Configure(EntityTypeBuilder<ComprobanteDetalleEntity> entity)
        {
            entity.ToTable("ComprobanteDetalle");
            entity.HasKey(e => e.ComprobanteDetalleID);
            entity.Property(e => e.Codigo).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.IdTipoIgv).HasMaxLength(10);
            entity.Property(e => e.TipoIgv).HasMaxLength(100);
            entity.Property(e => e.UnidadMedida).HasMaxLength(10);
            entity.Property(e => e.Valor).HasColumnType("decimal(12,2)");
            entity.Property(e => e.Cantidad).HasColumnType("decimal(12,2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(12,2)");
            entity.Property(e => e.Igv).HasColumnType("decimal(12,2)");
            entity.Property(e => e.Importe).HasColumnType("decimal(12,2)");
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.HasOne(d => d.Comprobante)
                .WithMany(c => c.Detalles)
                .HasForeignKey(d => d.ComprobanteID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
