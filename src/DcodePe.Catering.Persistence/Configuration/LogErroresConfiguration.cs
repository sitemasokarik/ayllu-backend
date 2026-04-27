using DcodePe.Catering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class LogErroresConfiguration
    {
        public LogErroresConfiguration(EntityTypeBuilder<LogErroresEntity> entityBuilder)
        {
            entityBuilder.HasKey(e => e.ErrorID).HasName("PK__LogError__358565CA1E55C3E9");

            entityBuilder.ToTable("LogErrores", "Auditoria");

            entityBuilder.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entityBuilder.Property(e => e.Mensaje).IsRequired();
            entityBuilder.Property(e => e.Usuario).HasMaxLength(100);

        }
    }
}
