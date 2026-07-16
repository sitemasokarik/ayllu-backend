using DcodePe.Catering.Domain.Entities.Pagos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class PagoVoucherConfiguration : IEntityTypeConfiguration<PagoVoucherEntity>
    {
        public void Configure(EntityTypeBuilder<PagoVoucherEntity> entityBuilder)
        {
            entityBuilder.ToTable("PagoVoucher");
            entityBuilder.HasKey(e => e.PagoVoucherID);

            entityBuilder.Property(e => e.ArchivoUrl).IsRequired().HasMaxLength(500);
            entityBuilder.Property(e => e.NombreArchivo).HasMaxLength(255);
            entityBuilder.Property(e => e.EstadoPago).IsRequired().HasMaxLength(30);
            entityBuilder.Property(e => e.ObservacionCliente).HasMaxLength(500);
            entityBuilder.Property(e => e.ObservacionAdmin).HasMaxLength(500);
            entityBuilder.Property(e => e.Monto).HasColumnType("decimal(12, 2)");
            entityBuilder.Property(e => e.FechaReservadaElegida).HasColumnType("datetime");
            entityBuilder.Property(e => e.Estado).HasDefaultValue(true);
            entityBuilder.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entityBuilder.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entityBuilder.Property(e => e.UsuarioCreacion).HasMaxLength(100);
            entityBuilder.Property(e => e.UsuarioModificacion).HasMaxLength(100);

            entityBuilder.HasOne(e => e.Cotizacion)
                .WithMany()
                .HasForeignKey(e => e.CotizacionID)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.ClienteID)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasIndex(e => e.EstadoPago).HasDatabaseName("IX_PagoVoucher_EstadoPago");
            entityBuilder.HasIndex(e => e.CotizacionID).HasDatabaseName("IX_PagoVoucher_CotizacionID");
        }
    }
}
