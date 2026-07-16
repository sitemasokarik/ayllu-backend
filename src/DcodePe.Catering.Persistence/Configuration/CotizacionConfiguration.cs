using DcodePe.Catering.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class CotizacionConfiguration
    {

        public CotizacionConfiguration(EntityTypeBuilder<CotizacionEntity> entityBuilder) {

            entityBuilder.HasKey(e => e.CotizacionID).HasName("PK__Cotizaci__30443A5921D82495");

            entityBuilder.ToTable("Cotizacion");
            
            // Configuración de propiedades decimales con nomenclatura actualizada
            entityBuilder.Property(e => e.CostoDePersonal)
                .HasColumnType("decimal(10, 2)");
            
            entityBuilder.Property(e => e.Garantia)
                .HasColumnType("decimal(10, 2)");
            
            entityBuilder.Property(e => e.TarifaMenuPorInvitado)
                .HasColumnType("decimal(10, 2)");
            
            entityBuilder.Property(e => e.SubtotalMenu)
                .HasColumnType("decimal(10, 2)");
            
            entityBuilder.Property(e => e.TotalEvento)
                .HasColumnType("decimal(10, 2)");
            
            entityBuilder.Property(e => e.PrecioPorCubierto)
                .HasColumnType("decimal(10, 2)");
            
            entityBuilder.Property(e => e.PrecioPorCubiertoConDescuento)
                .HasColumnType("decimal(10, 2)");
            
            entityBuilder.Property(e => e.TotalCotizacion)
                .HasColumnType("decimal(10, 2)");
            
            entityBuilder.Property(e => e.Estado)
                .HasDefaultValue(true);
            
            entityBuilder.Property(e => e.EstadoCotizacion)
                .IsRequired()
                .HasMaxLength(50);

            entityBuilder.Property(e => e.OrigenCotizacion)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValue("Admin");
            
            entityBuilder.Property(e => e.FechaTentativa)
                .HasColumnType("datetime");
            
            entityBuilder.Property(e => e.FechaTentativaOpcional)
                .HasColumnType("datetime");

            entityBuilder.Property(e => e.FechaReservada)
                .HasColumnType("datetime");
            
            entityBuilder.Property(e => e.FechaContacto)
                .HasColumnType("datetime");
            
            entityBuilder.Property(e => e.Observacion)
                .HasMaxLength(1000);

            entityBuilder.Property(e => e.BorradorJson)
                .HasColumnType("nvarchar(max)");

            entityBuilder.Property(e => e.CreadoPorNombre)
                .HasMaxLength(100);

            entityBuilder.Property(e => e.ResponsableNombre)
                .HasMaxLength(100);

            entityBuilder.Property(e => e.FechaAsignacion)
                .HasColumnType("datetime");
            
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

            // Relacion con Cliente
            entityBuilder.HasOne(d => d.Cliente)
                .WithMany(p => p.Cotizaciones)
                .HasForeignKey(d => d.ClienteID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Cotizacion_Cliente");

            // Relacion con Local - El cliente escoge el local para su evento
            entityBuilder.HasOne(d => d.Local)
                .WithMany(p => p.Cotizaciones)
                .HasForeignKey(d => d.LocalID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Cotizacion_Local");

            // Relacion con Evento - Una cotizacion pertenece a un evento
            entityBuilder.HasOne(d => d.Evento)
                .WithMany(p => p.Cotizaciones)
                .HasForeignKey(d => d.EventoID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Cotizacion_Evento");

            // Indexes
            entityBuilder.HasIndex(e => e.ClienteID)
                .HasDatabaseName("IX_Cotizacion_ClienteID");

            entityBuilder.HasIndex(e => e.LocalID)
                .HasDatabaseName("IX_Cotizacion_LocalID");

            entityBuilder.HasIndex(e => e.EventoID)
                .HasDatabaseName("IX_Cotizacion_EventoID");

            entityBuilder.HasIndex(e => e.EstadoCotizacion)
                .HasDatabaseName("IX_Cotizacion_EstadoCotizacion");
            
            entityBuilder.HasIndex(e => e.FechaTentativa)
                .HasDatabaseName("IX_Cotizacion_FechaTentativa");
        }
    }
}
