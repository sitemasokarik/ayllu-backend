using DcodePe.Catering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class CotizacionProductoConfiguration
    {
        public CotizacionProductoConfiguration(EntityTypeBuilder<CotizacionProductoEntity> entityBuilder)
        {
            entityBuilder.HasKey(e => new { e.CotizacionID, e.ProductoID }).HasName("PK__Cotizaci__1A0730B1F102918F");

            entityBuilder.ToTable("CotizacionProducto");

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

            entityBuilder.HasOne(d => d.Cotizacion).WithMany(p => p.CotizacionProducto)
                .HasForeignKey(d => d.CotizacionID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cotizacio__Cotiz__787EE5A0");

            entityBuilder.HasOne(d => d.Producto).WithMany(p => p.CotizacionProducto)
                .HasForeignKey(d => d.ProductoID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cotizacio__Produ__797309D9");
        }
    }
}
