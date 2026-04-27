using DcodePe.Catering.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class ContactanosConfiguration
    {
        public ContactanosConfiguration(EntityTypeBuilder<ContactanosEntity> entityBuilder)
        {
            entityBuilder.HasKey(e => e.ContactanosID)
                .HasName("PK__Contactanos");

            entityBuilder.ToTable("Contactanos");

            entityBuilder.Property(e => e.NombreCompleto)
                .IsRequired()
                .HasMaxLength(200);

            entityBuilder.Property(e => e.Correo)
                .IsRequired()
                .HasMaxLength(100);

            entityBuilder.Property(e => e.Telefono)
                .IsRequired()
                .HasMaxLength(20);

            entityBuilder.Property(e => e.Servicio)
                .IsRequired()
                .HasMaxLength(100);

            entityBuilder.Property(e => e.Mensaje)
                .IsRequired()
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
                .HasDatabaseName("IX_Contactanos_Estado");

            entityBuilder.HasIndex(e => e.Correo)
                .HasDatabaseName("IX_Contactanos_Correo");

            entityBuilder.HasIndex(e => e.FechaCreacion)
                .HasDatabaseName("IX_Contactanos_FechaCreacion");
        }
    }
}
