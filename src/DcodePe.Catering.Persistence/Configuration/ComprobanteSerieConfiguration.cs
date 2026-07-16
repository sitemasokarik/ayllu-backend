using DcodePe.Catering.Domain.Entities.Facturacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class ComprobanteSerieConfiguration : IEntityTypeConfiguration<ComprobanteSerieEntity>
    {
        public void Configure(EntityTypeBuilder<ComprobanteSerieEntity> entity)
        {
            entity.ToTable("ComprobanteSerie");
            entity.HasKey(e => e.ComprobanteSerieID);
            entity.Property(e => e.Tipo).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Serie).HasMaxLength(10).IsRequired();
            entity.HasIndex(e => e.Serie).IsUnique();
        }
    }
}
