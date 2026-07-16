using DcodePe.Catering.Domain.Entities.Pagos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class PagoMercadoPagoConfiguration : IEntityTypeConfiguration<PagoMercadoPagoEntity>
    {
        public void Configure(EntityTypeBuilder<PagoMercadoPagoEntity> entity)
        {
            entity.ToTable("PagoMercadoPago");
            entity.HasKey(e => e.PagoMercadoPagoID);
            entity.Property(e => e.Monto).HasColumnType("decimal(12,2)");
            entity.Property(e => e.FechaReservadaElegida).HasColumnType("datetime");
            entity.Property(e => e.MpPaymentId).HasMaxLength(50).IsRequired();
            entity.Property(e => e.MpPreferenceId).HasMaxLength(50).IsRequired();
            entity.Property(e => e.EstadoPago).HasMaxLength(30).IsRequired();
            entity.Property(e => e.MpStatusDetail).HasMaxLength(200);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.HasIndex(e => e.MpPaymentId).IsUnique().HasDatabaseName("UQ_PagoMercadoPago_MpPaymentId");
            entity.HasIndex(e => e.CotizacionID).HasDatabaseName("IX_PagoMercadoPago_CotizacionID");
            entity.HasIndex(e => e.EstadoPago).HasDatabaseName("IX_PagoMercadoPago_EstadoPago");

            entity.HasOne(e => e.Cotizacion)
                .WithMany()
                .HasForeignKey(e => e.CotizacionID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.ClienteID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
