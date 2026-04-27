using DcodePe.Catering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class LocalConfiguration
    {
        public LocalConfiguration(EntityTypeBuilder<LocalEntity> entityBuilder)
        {
            entityBuilder.HasKey(e => e.LocalID).HasName("PK__Local__499359DBAF7800EF");

            entityBuilder.ToTable("Local");

            entityBuilder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);
            
            entityBuilder.Property(e => e.Direccion)
                .IsRequired()
                .HasMaxLength(255);
            
            entityBuilder.Property(e => e.Capacidad)
                .IsRequired();
            
            entityBuilder.Property(e => e.PrecioAlquiler)
                .HasColumnType("decimal(10, 2)")
                .IsRequired();
            
            entityBuilder.Property(e => e.HorasEvento)
                .HasColumnType("decimal(5, 2)")
                .IsRequired();
            
            entityBuilder.Property(e => e.Fotos)
                .HasMaxLength(2000);
            
            entityBuilder.Property(e => e.TerminosCondiciones)
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
                .HasDatabaseName("IX_Local_Estado");
            
            entityBuilder.HasIndex(e => e.Capacidad)
                .HasDatabaseName("IX_Local_Capacidad");
        }
    }
}
