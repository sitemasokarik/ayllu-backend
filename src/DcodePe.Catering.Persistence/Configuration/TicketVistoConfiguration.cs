using DcodePe.Catering.Domain.Entities.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class TicketVistoConfiguration : IEntityTypeConfiguration<TicketVistoEntity>
    {
        public void Configure(EntityTypeBuilder<TicketVistoEntity> entity)
        {
            entity.ToTable("TicketVisto");
            entity.HasKey(e => e.TicketVistoID);
            entity.Property(e => e.FechaVisto).HasDefaultValueSql("(getdate())");
            entity.HasIndex(e => new { e.TicketID, e.UsuarioID }).IsUnique();
        }
    }
}
