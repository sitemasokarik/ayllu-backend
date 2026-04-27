using DcodePe.Catering.Domain.Entities;

namespace DcodePe.Catering.Persistence.Configuration
{
    public  class UsuarioConfiguration
    {
        public UsuarioConfiguration(EntityTypeBuilder<UsuarioEntity> entityBuilder) {

            entityBuilder.HasKey(e => e.UsuarioID).HasName("PK__Usuario__2B3DE7985AB70077");

            entityBuilder.ToTable("Usuario");

            entityBuilder.HasIndex(e => e.Email, "UQ__Usuario__A9D1053408714BD2").IsUnique();

            entityBuilder.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255);
            entityBuilder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
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

            entityBuilder.HasOne(d => d.Rol).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.RolID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__RolID__4BAC3F29");
        }
    }
}
