using DcodePe.Catering.Domain.Entities.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class TicketInternoConfiguration : IEntityTypeConfiguration<TicketInternoEntity>
    {
        public void Configure(EntityTypeBuilder<TicketInternoEntity> entity)
        {
            entity.ToTable("TicketInterno");
            entity.HasKey(e => e.TicketID);
            entity.Property(e => e.Titulo).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(2000);
            entity.Property(e => e.EstadoTicket).HasMaxLength(30);
            entity.Property(e => e.Prioridad).HasMaxLength(20);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");
        }
    }
}
