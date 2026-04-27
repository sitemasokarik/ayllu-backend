using DcodePe.Catering.Domain.Entities;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class CotizacionServicioConfiguration
    {
        public CotizacionServicioConfiguration(EntityTypeBuilder<CotizacionServicioEntity> entityBuilder)
        {
            entityBuilder.HasKey(e => new { e.CotizacionID, e.ServicioID })
                .HasName("PK__Cotizaci__0D1ED49B07A984DD");

            entityBuilder.ToTable("CotizacionServicio");

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
                .WithMany(p => p.CotizacionServicio)
                .HasForeignKey(d => d.CotizacionID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cotizacio__Cotiz__72C60C4A");

            entityBuilder.HasOne(d => d.Servicio)
                .WithMany(p => p.CotizacionServicio)
                .HasForeignKey(d => d.ServicioID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cotizacio__Servi__73BA3083");
        }
    }
}
