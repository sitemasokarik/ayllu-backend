using DcodePe.Catering.Domain.Entities;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class PaqueteProductoConfiguration
    {

        public PaqueteProductoConfiguration(EntityTypeBuilder<PaqueteProductoEntity> entityBuilder)
        {
            entityBuilder.HasKey(e => new { e.PaqueteID, e.ProductoID }).HasName("PK__PaqueteP__51DC273AB8D4D98B");

            entityBuilder.ToTable("PaqueteProducto");

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

            entityBuilder.HasOne(d => d.Paquete).WithMany(p => p.PaqueteProducto)
                .HasForeignKey(d => d.PaqueteID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaquetePr__Paque__7E37BEF6");

            entityBuilder.HasOne(d => d.Producto).WithMany(p => p.PaqueteProducto)
                .HasForeignKey(d => d.ProductoID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaquetePr__Produ__7F2BE32F");
        }
    }
}
