using DcodePe.Catering.Domain.Entities.Clientes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DcodePe.Catering.Persistence.Configuration
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Cliente
    /// </summary>
    public class ClienteConfiguration : IEntityTypeConfiguration<ClienteEntity>
    {
        public void Configure(EntityTypeBuilder<ClienteEntity> entityBuilder)
        {
            // Primary Key
            entityBuilder.HasKey(e => e.ClienteID)
                .HasName("PK_Cliente");

            // Table Name
            entityBuilder.ToTable("Cliente");

            // Properties
            entityBuilder.Property(e => e.TipoDocumento)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("Tipo de documento: DNI, RUC, Pasaporte, Carnet de Extranjería");

            entityBuilder.Property(e => e.NumeroDocumento)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Número de documento de identidad");

            entityBuilder.Property(e => e.NombreCompleto)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("Nombre completo o razón social del cliente");

            entityBuilder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("Correo electrónico del cliente");

            entityBuilder.Property(e => e.Telefono)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Número de teléfono principal");

            entityBuilder.Property(e => e.TelefonoSecundario)
                .HasMaxLength(20)
                .HasComment("Número de teléfono secundario o celular");

            entityBuilder.Property(e => e.Direccion)
                .HasMaxLength(500)
                .HasComment("Dirección completa del cliente");

            entityBuilder.Property(e => e.Ciudad)
                .HasMaxLength(100)
                .HasComment("Ciudad del cliente");

            entityBuilder.Property(e => e.Pais)
                .HasMaxLength(100)
                .HasDefaultValue("Perú")
                .HasComment("País del cliente");

            entityBuilder.Property(e => e.TipoCliente)
                .HasMaxLength(50)
                .HasDefaultValue("Particular")
                .HasComment("Tipo de cliente: Particular, Empresa, Gobierno");

            entityBuilder.Property(e => e.Observaciones)
                .HasMaxLength(1000)
                .HasComment("Observaciones o notas adicionales");

            entityBuilder.Property(e => e.EsVIP)
                .HasDefaultValue(false)
                .HasComment("Indica si el cliente es VIP");

            entityBuilder.Property(e => e.FechaNacimiento)
                .HasColumnType("date")
                .HasComment("Fecha de nacimiento o constitución");

            entityBuilder.Property(e => e.UserNamePortal)
                .HasMaxLength(100);

            entityBuilder.Property(e => e.PasswordHash)
                .HasMaxLength(255);

            entityBuilder.Property(e => e.EsPortalActivo)
                .HasDefaultValue(false);

            entityBuilder.HasIndex(e => e.UserNamePortal)
                .IsUnique()
                .HasFilter("[UserNamePortal] IS NOT NULL")
                .HasDatabaseName("UQ_Cliente_UserNamePortal");

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
                .IsRequired()
                .HasMaxLength(100);

            entityBuilder.Property(e => e.UsuarioEliminacion)
                .HasMaxLength(100);

            entityBuilder.Property(e => e.UsuarioModificacion)
                .HasMaxLength(100);

            // Relationships
            // Un cliente puede tener muchas cotizaciones
            entityBuilder.HasMany(c => c.Cotizaciones)
                .WithOne(cot => cot.Cliente)
                .HasForeignKey(cot => cot.ClienteID)
                .OnDelete(DeleteBehavior.Restrict) // No permitir eliminar cliente si tiene cotizaciones
                .HasConstraintName("FK_Cotizacion_Cliente");

            // Indexes
            entityBuilder.HasIndex(e => e.NumeroDocumento)
                .IsUnique()
                .HasDatabaseName("UQ_Cliente_NumeroDocumento");

            entityBuilder.HasIndex(e => e.Email)
                .HasDatabaseName("IX_Cliente_Email");

            entityBuilder.HasIndex(e => e.Estado)
                .HasDatabaseName("IX_Cliente_Estado");

            entityBuilder.HasIndex(e => e.TipoCliente)
                .HasDatabaseName("IX_Cliente_TipoCliente");

            entityBuilder.HasIndex(e => e.EsVIP)
                .HasDatabaseName("IX_Cliente_EsVIP");
        }
    }
}
