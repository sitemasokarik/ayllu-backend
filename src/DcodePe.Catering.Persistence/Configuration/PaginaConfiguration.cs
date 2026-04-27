using DcodePe.Catering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Persistence.Configuration
{
    public class PaginaConfiguration
    {
        public PaginaConfiguration(EntityTypeBuilder<PaginaEntity> entityBuilder)
        {

            entityBuilder.HasKey(e => e.PaginaID).HasName("PK__Pagina__31EFFA4D5845C0D1");

            entityBuilder.ToTable("Pagina");

            entityBuilder.Property(e => e.Descripcion).HasMaxLength(255);
            entityBuilder.Property(e => e.Url).HasMaxLength(500);
            entityBuilder.Property(e => e.Icono).HasMaxLength(100);
            entityBuilder.Property(e => e.Estado).HasDefaultValue(true);
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
        }
    }
}
