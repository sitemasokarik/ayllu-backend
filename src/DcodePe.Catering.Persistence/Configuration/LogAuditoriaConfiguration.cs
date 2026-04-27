using DcodePe.Catering.Domain.Entities;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class LogAuditoriaConfiguration
    {
        public LogAuditoriaConfiguration(EntityTypeBuilder<LogAuditoriaEntity> entityBuilder) {

            entityBuilder.HasKey(e => e.LogID).HasName("PK__LogAudit__5E5499A8B9526321");

            entityBuilder.ToTable("LogAuditoria", "Auditoria");

            entityBuilder.Property(e => e.Acción)
                .IsRequired()
                .HasMaxLength(100);
            entityBuilder.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entityBuilder.Property(e => e.TablaAfectada)
                .IsRequired()
                .HasMaxLength(100);
            entityBuilder.Property(e => e.Usuario)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
