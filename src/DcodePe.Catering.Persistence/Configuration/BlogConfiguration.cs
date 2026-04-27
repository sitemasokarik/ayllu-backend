using DcodePe.Catering.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class BlogConfiguration
    {
        public BlogConfiguration(EntityTypeBuilder<BlogEntity> entityBuilder)
        {
            entityBuilder.HasKey(e => e.BlogID)
                .HasName("PK__Blog");

            entityBuilder.ToTable("Blog");

            entityBuilder.Property(e => e.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            entityBuilder.Property(e => e.Descripcion)
                .IsRequired()
                .HasMaxLength(2000);

            entityBuilder.Property(e => e.ValoresJson)
                .HasColumnType("nvarchar(max)");

            entityBuilder.Property(e => e.Imagenes)
                .HasMaxLength(2000);

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

            entityBuilder.HasIndex(e => e.Estado)
                .HasDatabaseName("IX_Blog_Estado");

            entityBuilder.HasIndex(e => e.Titulo)
                .HasDatabaseName("IX_Blog_Titulo");
        }
    }
}
