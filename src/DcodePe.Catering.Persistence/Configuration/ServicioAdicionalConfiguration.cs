using DcodePe.Catering.Domain.Entities;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class ServicioAdicionalConfiguration
    {
        public ServicioAdicionalConfiguration(EntityTypeBuilder<ServicioAdicionalEntity> entity)
        {
            entity.HasKey(e => e.ServicioID).HasName("PK__Servicio__D5AEEC2230B36B62");

            entity.ToTable("ServicioAdicional");

            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaEliminacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CantidadMinima).HasDefaultValue(1);
            entity.Property(e => e.Fotos)
             .HasMaxLength(500);
            entity.Property(e => e.UsuarioCreacion).HasMaxLength(100);
            entity.Property(e => e.UsuarioEliminacion).HasMaxLength(100);
            entity.Property(e => e.UsuarioModificacion).HasMaxLength(100);
        }

    }
}
