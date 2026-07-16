using DcodePe.Catering.Domain.Entities;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class EventoConfiguration
    {
        public EventoConfiguration(EntityTypeBuilder<EventoEntity> entityBuilder) {

            entityBuilder.HasKey(e => e.EventoID).HasName("PK__Evento__1EEB5901FC5279F4");

            entityBuilder.ToTable("Evento");

            entityBuilder.Property(e => e.Descripcion).HasMaxLength(255);
            
            entityBuilder.Property(e => e.Fotos)
                .HasMaxLength(500)
                .HasComment("URL de la foto del evento");
            
            entityBuilder.Property(e => e.Estado).HasDefaultValue(true);
            
            entityBuilder.Property(e => e.EstadoEvento)
                .IsRequired()
                .HasMaxLength(50);
            
            entityBuilder.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            
            entityBuilder.Property(e => e.FechaEliminacion).HasColumnType("datetime");
            
            entityBuilder.Property(e => e.FechaModificacion).HasColumnType("datetime");
            
            entityBuilder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);
            
            entityBuilder.Property(e => e.UsuarioCreacion).HasMaxLength(100);
            
            entityBuilder.Property(e => e.UsuarioEliminacion).HasMaxLength(100);
            
            entityBuilder.Property(e => e.UsuarioModificacion).HasMaxLength(100);

            entityBuilder.Property(e => e.TarifasInvitadoJson)
                .HasColumnType("TEXT");

        }
    }
}
