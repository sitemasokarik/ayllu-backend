using DcodePe.Catering.Domain.Entities;
using DcodePe.Catering.Domain.Entities.Clientes;
using DcodePe.Catering.Domain.Entities.Facturacion;
using DcodePe.Catering.Domain.Entities.Pagos;
using DcodePe.Catering.Domain.Entities.Tickets;
using DcodePe.Catering.Persistence.Configuration;

namespace DcodePe.Catering.Persistence.DataBase
{
    public class DataBaseService: DbContext, IDataBaseService
    {
        public static bool UseSqliteProvider { get; set; }

        public DataBaseService(DbContextOptions options): base(options)
        {
        }
        public DbSet<UserEntity> User {  get; set; }
        public DbSet<CustomerEntity> Customer { get; set; }
        public DbSet<BookingEntity> Booking { get; set; }
        public DbSet<BlogEntity> Blog { get; set; }
        public DbSet<ClienteEntity> Cliente { get; set; }
        public DbSet<CotizacionEntity> Cotizacion { get; set; }
        public DbSet<CotizacionProductoEntity> CotizacionProducto { get; set; }
        public DbSet<CotizacionServicioEntity> CotizacionServicio { get; set; }
        public DbSet<CotizacionPaqueteEntity> CotizacionPaquete { get; set; }
        public DbSet<EventoEntity> Evento { get; set; }
        public DbSet<LocalEntity> Local { get; set; }
        public DbSet<LogAuditoriaEntity> LogAuditoria { get; set; }
        public DbSet<LogErroresEntity> LogErrores { get; set; }
        public DbSet<PaginaEntity> Pagina { get; set; }
        public DbSet<PaqueteEntity> Paquete { get; set; }
        public DbSet<PaqueteProductoEntity> PaqueteProducto { get; set; }
        public DbSet<PaqueteServicioEntity> PaqueteServicio { get; set; }
        public DbSet<PermisoEntity> Permiso { get; set; }
        public DbSet<ProductoEntity> Producto { get; set; }
        public DbSet<CategoriaEntity> Categoria { get; set; }
        public DbSet<RolEntity> Rol { get; set; }
        public DbSet<ServicioAdicionalEntity> ServicioAdicional { get; set; }
        public DbSet<UsuarioEntity> Usuario { get; set; }
        public DbSet<EmpresaEntity> Empresa { get; set; }
        public DbSet<ContactanosEntity> Contactanos { get; set; }
        public DbSet<ComprobanteElectronicoEntity> ComprobanteElectronico { get; set; }
        public DbSet<ComprobanteDetalleEntity> ComprobanteDetalle { get; set; }
        public DbSet<ComprobanteSerieEntity> ComprobanteSerie { get; set; }
        public DbSet<TicketInternoEntity> TicketInterno { get; set; }
        public DbSet<TicketMensajeEntity> TicketMensaje { get; set; }
        public DbSet<TicketVistoEntity> TicketVisto { get; set; }
        public DbSet<PagoVoucherEntity> PagoVoucher { get; set; }
        public DbSet<PagoMercadoPagoEntity> PagoMercadoPago { get; set; }

        public async Task<bool> SaveAsync()
        {
            return await SaveChangesAsync() > 0 ;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configuración de la clave primaria para BookingEntity
            modelBuilder.Entity<BookingEntity>()
                .HasKey(b => b.BookingId);

            // ✅ Aplicar configuraciones con ApplyConfiguration
            modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
            modelBuilder.ApplyConfiguration(new ProductoConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new ComprobanteElectronicoConfiguration());
            modelBuilder.ApplyConfiguration(new ComprobanteDetalleConfiguration());
            modelBuilder.ApplyConfiguration(new ComprobanteSerieConfiguration());
            modelBuilder.ApplyConfiguration(new TicketInternoConfiguration());
            modelBuilder.ApplyConfiguration(new TicketMensajeConfiguration());
            modelBuilder.ApplyConfiguration(new TicketVistoConfiguration());
            modelBuilder.ApplyConfiguration(new PagoVoucherConfiguration());
            modelBuilder.ApplyConfiguration(new PagoMercadoPagoConfiguration());

            modelBuilder.Entity<PaqueteEntity>(entity =>
            {
                entity.HasKey(e => e.PaqueteID).HasName("PK__Paquete__7B9F2DD2F0612323");

                entity.ToTable("Paquete");

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
                entity.Property(e => e.PrecioTotal).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.UsuarioCreacion).HasMaxLength(100);
                entity.Property(e => e.UsuarioEliminacion).HasMaxLength(100);
                entity.Property(e => e.UsuarioModificacion).HasMaxLength(100);
            });

            modelBuilder.Entity<PaqueteProductoEntity>(entity =>
            {
                entity.HasKey(e => new { e.PaqueteID, e.ProductoID }).HasName("PK__PaqueteP__51DC273AB8D4D98B");

                entity.ToTable("PaqueteProducto");

                entity.Property(e => e.Estado).HasDefaultValue(true);
                entity.Property(e => e.FechaCreacion)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.FechaEliminacion).HasColumnType("datetime");
                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
                entity.Property(e => e.UsuarioCreacion).HasMaxLength(100);
                entity.Property(e => e.UsuarioEliminacion).HasMaxLength(100);
                entity.Property(e => e.UsuarioModificacion).HasMaxLength(100);

                entity.HasOne(d => d.Paquete).WithMany(p => p.PaqueteProducto)
                    .HasForeignKey(d => d.PaqueteID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PaquetePr__Paque__7E37BEF6");

                entity.HasOne(d => d.Producto).WithMany(p => p.PaqueteProducto)
                    .HasForeignKey(d => d.ProductoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PaquetePr__Produ__7F2BE32F");
            });

            // Configuración de otras entidades
            EntityConfiguration(modelBuilder);

            if (UseSqliteProvider)
                SqliteCompatibility.Apply(modelBuilder);
        }
        
        private void EntityConfiguration(ModelBuilder modelBuilder)
        {
            new UserConfiguration(modelBuilder.Entity<UserEntity>());
            new CustomerConfiguration(modelBuilder.Entity<CustomerEntity>());
            new BookingConfiguration(modelBuilder.Entity<BookingEntity>());
            new BlogConfiguration(modelBuilder.Entity<BlogEntity>());
            new CotizacionConfiguration(modelBuilder.Entity<CotizacionEntity>());
            new CotizacionProductoConfiguration(modelBuilder.Entity<CotizacionProductoEntity>());
            new CotizacionServicioConfiguration(modelBuilder.Entity<CotizacionServicioEntity>());
            new CotizacionPaqueteConfiguration(modelBuilder.Entity<CotizacionPaqueteEntity>());
            new EventoConfiguration(modelBuilder.Entity<EventoEntity>());
            new LocalConfiguration(modelBuilder.Entity<LocalEntity>());
            new LogAuditoriaConfiguration(modelBuilder.Entity<LogAuditoriaEntity>());
            new LogErroresConfiguration(modelBuilder.Entity<LogErroresEntity>());
            new PaginaConfiguration(modelBuilder.Entity<PaginaEntity>());
            new PaqueteConfiguration(modelBuilder.Entity<PaqueteEntity>());
            new PaqueteProductoConfiguration(modelBuilder.Entity<PaqueteProductoEntity>());
            new PaqueteServicioConfiguration(modelBuilder.Entity<PaqueteServicioEntity>());
            new PermisoConfiguration(modelBuilder.Entity<PermisoEntity>());
            // new ProductoConfiguration(modelBuilder.Entity<ProductoEntity>()); // ❌ ELIMINADO - ya se aplica con ApplyConfiguration
            new RolConfiguration(modelBuilder.Entity<RolEntity>());
            new ServicioAdicionalConfiguration(modelBuilder.Entity<ServicioAdicionalEntity>());
            new UsuarioConfiguration(modelBuilder.Entity<UsuarioEntity>());
            new EmpresaConfiguration(modelBuilder.Entity<EmpresaEntity>());
            new ContactanosConfiguration(modelBuilder.Entity<ContactanosEntity>());
        }
    }
}
