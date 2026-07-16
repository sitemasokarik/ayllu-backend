using DcodePe.Catering.Domain.Entities.Facturacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class ComprobanteElectronicoConfiguration : IEntityTypeConfiguration<ComprobanteElectronicoEntity>
    {
        public void Configure(EntityTypeBuilder<ComprobanteElectronicoEntity> entity)
        {
            entity.ToTable("ComprobanteElectronico");
            entity.HasKey(e => e.ComprobanteID);
            entity.Property(e => e.Tipo).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Serie).HasMaxLength(10).IsRequired();
            entity.Property(e => e.Correlativo).HasMaxLength(20).IsRequired();
            entity.Property(e => e.NumeroCompleto).HasMaxLength(30).IsRequired();
            entity.Property(e => e.ClienteNombre).HasMaxLength(200).IsRequired();
            entity.Property(e => e.ClienteDocumento).HasMaxLength(20).IsRequired();
            entity.Property(e => e.TipoDocumento).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ClienteDireccion).HasMaxLength(500);
            entity.Property(e => e.ClienteTelefono).HasMaxLength(30);
            entity.Property(e => e.FormaPago).HasMaxLength(50);
            entity.Property(e => e.MedioPago).HasMaxLength(50);
            entity.Property(e => e.Moneda).HasMaxLength(10);
            entity.Property(e => e.ModoEmision).HasMaxLength(20);
            entity.Property(e => e.EstadoComprobante).HasMaxLength(30);
            entity.Property(e => e.SunatTicket).HasMaxLength(100);
            entity.Property(e => e.SunatCdr).HasMaxLength(100);
            entity.Property(e => e.SunatRespuesta).HasMaxLength(2000);
            entity.Property(e => e.SunatHashCpe).HasMaxLength(200);
            entity.Property(e => e.RutaXml).HasMaxLength(500);
            entity.Property(e => e.RutaCdr).HasMaxLength(500);
            entity.Property(e => e.SunatCodigoError).HasMaxLength(50);
            entity.Property(e => e.MontoAdelantoFacturado).HasColumnType("decimal(12,2)");
            entity.Property(e => e.OpGravadas).HasColumnType("decimal(12,2)");
            entity.Property(e => e.OpInafectas).HasColumnType("decimal(12,2)");
            entity.Property(e => e.OpExoneradas).HasColumnType("decimal(12,2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(12,2)");
            entity.Property(e => e.Igv).HasColumnType("decimal(12,2)");
            entity.Property(e => e.Total).HasColumnType("decimal(12,2)");
            entity.Property(e => e.Recibido).HasColumnType("decimal(12,2)");
            entity.Property(e => e.Vuelto).HasColumnType("decimal(12,2)");
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");
            entity.HasIndex(e => e.NumeroCompleto).IsUnique();
        }
    }
}
