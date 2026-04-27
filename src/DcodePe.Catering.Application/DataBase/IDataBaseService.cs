using DcodePe.Catering.Domain.Entities;
using DcodePe.Catering.Domain.Entities.Clientes;

namespace DcodePe.Catering.Application.Database
{
    public interface IDataBaseService
    {
        DbSet<UserEntity> User { get; set; }
        DbSet<CustomerEntity> Customer { get; set; }
        DbSet<BookingEntity> Booking { get; set; }
        DbSet<LocalEntity> Local { get; set; }
        DbSet<EventoEntity> Evento { get; set; }
        DbSet<UsuarioEntity> Usuario { get; set; }
        DbSet<ClienteEntity> Cliente { get; set; }
        DbSet<ProductoEntity> Producto { get; set; }
        DbSet<CategoriaEntity> Categoria { get; set; }
        DbSet<PaqueteEntity> Paquete { get; set; }
        DbSet<PaqueteProductoEntity> PaqueteProducto { get; set; }
        DbSet<CotizacionEntity> Cotizacion { get; set; }
        DbSet<CotizacionProductoEntity> CotizacionProducto { get; set; }
        DbSet<CotizacionServicioEntity> CotizacionServicio { get; set; }
        DbSet<CotizacionPaqueteEntity> CotizacionPaquete { get; set; }
        DbSet<ServicioAdicionalEntity> ServicioAdicional { get; set; }
        DbSet<PaqueteServicioEntity> PaqueteServicio { get; set; }
        DbSet<BlogEntity> Blog { get; set; }
        DbSet<EmpresaEntity> Empresa { get; set; }
        DbSet<ContactanosEntity> Contactanos { get; set; }
        DbSet<RolEntity> Rol { get; set; }
        DbSet<PaginaEntity> Pagina { get; set; }
        DbSet<PermisoEntity> Permiso { get; set; }

        Task<bool> SaveAsync();
    }
}
