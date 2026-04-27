using DcodePe.Catering.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class EmpresaConfiguration
    {
        public EmpresaConfiguration(EntityTypeBuilder<EmpresaEntity> entityBuilder)
        {
            entityBuilder.HasKey(e => e.EmpresaID)
                .HasName("PK__Empresa");

            entityBuilder.ToTable("Empresa");

            entityBuilder.Property(e => e.RazonSocial)
                .IsRequired()
                .HasMaxLength(200);

            entityBuilder.Property(e => e.NombreComercial)
                .IsRequired()
                .HasMaxLength(200);

            entityBuilder.Property(e => e.RUC)
                .IsRequired()
                .HasMaxLength(20);

            entityBuilder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            entityBuilder.Property(e => e.Telefono)
                .IsRequired()
                .HasMaxLength(20);

            entityBuilder.Property(e => e.TelefonoSecundario)
                .HasMaxLength(20);

            entityBuilder.Property(e => e.WhatsApp)
                .HasMaxLength(20);

            entityBuilder.Property(e => e.Direccion)
                .IsRequired()
                .HasMaxLength(500);

            entityBuilder.Property(e => e.Ciudad)
                .HasMaxLength(100);

            entityBuilder.Property(e => e.Pais)
                .HasMaxLength(100)
                .HasDefaultValue("Perú");

            entityBuilder.Property(e => e.Facebook)
                .HasMaxLength(200);

            entityBuilder.Property(e => e.Instagram)
                .HasMaxLength(200);

            entityBuilder.Property(e => e.LinkedIn)
                .HasMaxLength(200);

            entityBuilder.Property(e => e.Twitter)
                .HasMaxLength(200);

            entityBuilder.Property(e => e.HorarioAtencion)
                .HasMaxLength(200);

            entityBuilder.Property(e => e.Logo)
                .HasMaxLength(500);

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

            entityBuilder.HasIndex(e => e.RUC)
                .IsUnique()
                .HasDatabaseName("UQ_Empresa_RUC");

            entityBuilder.HasIndex(e => e.Email)
                .HasDatabaseName("IX_Empresa_Email");

            entityBuilder.HasIndex(e => e.Estado)
                .HasDatabaseName("IX_Empresa_Estado");
        }
    }
}
