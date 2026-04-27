using DcodePe.Catering.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class ProductoConfiguration : IEntityTypeConfiguration<ProductoEntity>
    {
        public void Configure(EntityTypeBuilder<ProductoEntity> entityBuilder)
        {
            // Primary Key
            entityBuilder.HasKey(e => e.ProductoID)
                .HasName("PK__Producto__A430AE838BF44EEC");

            // Table Name
            entityBuilder.ToTable("Producto");

            // Properties
            entityBuilder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            entityBuilder.Property(e => e.Descripcion)
                .HasMaxLength(255);

            entityBuilder.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .IsRequired();

            entityBuilder.Property(e => e.PrecioCosto)
                .HasColumnType("decimal(10, 2)")
                .IsRequired();

            entityBuilder.Property(e => e.Fotos)
             .HasMaxLength(500);

            // Foreign Key a Categoria
            entityBuilder.Property(e => e.CategoriaID)
                .IsRequired();

            // Audit Fields (heredados de BaseEntity)
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

            // Relationships
            entityBuilder.HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Producto_Categoria");

            // Indexes
            entityBuilder.HasIndex(e => e.CategoriaID)
                .HasDatabaseName("IX_Producto_CategoriaID");

            entityBuilder.HasIndex(e => e.Nombre)
                .HasDatabaseName("IX_Producto_Nombre");

            entityBuilder.HasIndex(e => e.Estado)
                .HasDatabaseName("IX_Producto_Estado");
        }
    }
}
