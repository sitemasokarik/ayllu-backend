using DcodePe.Catering.Domain.Entities.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class TicketMensajeConfiguration : IEntityTypeConfiguration<TicketMensajeEntity>
    {
        public void Configure(EntityTypeBuilder<TicketMensajeEntity> entity)
        {
            entity.ToTable("TicketMensaje");
            entity.HasKey(e => e.TicketMensajeID);
            entity.Property(e => e.AutorNombre).HasMaxLength(200);
            entity.Property(e => e.Mensaje).HasMaxLength(4000).IsRequired();
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");
            entity.HasOne(m => m.Ticket)
                .WithMany(t => t.Mensajes)
                .HasForeignKey(m => m.TicketID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
