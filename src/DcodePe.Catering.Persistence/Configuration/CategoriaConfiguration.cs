using DcodePe.Catering.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<CategoriaEntity>
    {
        public void Configure(EntityTypeBuilder<CategoriaEntity> entityBuilder)
        {
            // Primary Key
            entityBuilder.HasKey(e => e.CategoriaID)
                .HasName("PK_Categoria");

            // Table Name
            entityBuilder.ToTable("Categoria");

            // Properties
            entityBuilder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            entityBuilder.Property(e => e.Descripcion)
                .HasMaxLength(500);

            entityBuilder.Property(e => e.Nivel)
                .IsRequired()
                .HasDefaultValue(0);

            entityBuilder.Property(e => e.Orden)
                .IsRequired()
                .HasDefaultValue(0);

            entityBuilder.Property(e => e.Icono)
                .HasMaxLength(255);

            entityBuilder.Property(e => e.Limite)
                .IsRequired()
                .HasDefaultValue(0);

            // ===== AUTO-REFERENCIA: Relación Padre-Hijo (Recursiva) =====
            entityBuilder.HasOne(c => c.CategoriaPadre)
                .WithMany(c => c.Subcategorias)
                .HasForeignKey(c => c.CategoriaPadreID)
                .OnDelete(DeleteBehavior.Restrict) // Prevenir eliminación en cascada
                .HasConstraintName("FK_Categoria_CategoriaPadre");

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

            // Relationships con Productos
            entityBuilder.HasMany(c => c.Productos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaID)
                .OnDelete(DeleteBehavior.Restrict) // No permitir eliminar categoría si tiene productos
                .HasConstraintName("FK_Producto_Categoria");

            // Indexes
            entityBuilder.HasIndex(e => e.CategoriaPadreID)
                .HasDatabaseName("IX_Categoria_CategoriaPadreID");

            entityBuilder.HasIndex(e => e.Nombre)
                .HasDatabaseName("IX_Categoria_Nombre");

            entityBuilder.HasIndex(e => e.Nivel)
                .HasDatabaseName("IX_Categoria_Nivel");

            entityBuilder.HasIndex(e => new { e.CategoriaPadreID, e.Orden })
                .HasDatabaseName("IX_Categoria_Padre_Orden");

            entityBuilder.HasIndex(e => e.Estado)
                .HasDatabaseName("IX_Categoria_Estado");
        }
    }
}
