using DcodePe.Catering.Application.DataBase.Categoria.Commands.Create;
using DcodePe.Catering.Application.DataBase.Categoria.Commands.Update;
using DcodePe.Catering.Application.DataBase.Categoria.Queries.GetAll;
using DcodePe.Catering.Application.DataBase.Categoria.Queries.GetById;
using DcodePe.Catering.Application.DataBase.Cliente.Commands.Create;
using DcodePe.Catering.Application.DataBase.Cliente.Commands.Update;
using DcodePe.Catering.Application.DataBase.Cliente.Queries.GetAllCliente;
using DcodePe.Catering.Application.DataBase.Contactanos.Commands.Create;
using DcodePe.Catering.Application.DataBase.Contactanos.Queries.GetAllContactanos;
using DcodePe.Catering.Application.DataBase.Evento.Commands.Update;
using DcodePe.Catering.Application.DataBase.Local.Commands.Update;
using DcodePe.Catering.Application.DataBase.Pagina.Commands.Create;
using DcodePe.Catering.Application.DataBase.Pagina.Commands.Update;
using DcodePe.Catering.Application.DataBase.Pagina.Queries.GetAll;
using DcodePe.Catering.Application.DataBase.Permiso.Commands.Create;
using DcodePe.Catering.Application.DataBase.Permiso.Commands.Update;
using DcodePe.Catering.Application.DataBase.Producto.Commands.Update;
using DcodePe.Catering.Application.DataBase.Rol.Commands.Create;
using DcodePe.Catering.Application.DataBase.Rol.Commands.Update;
using DcodePe.Catering.Application.DataBase.Rol.Queries.GetAll;
using DcodePe.Catering.Application.DataBase.Rol.Queries.GetById;
using DcodePe.Catering.Application.DataBase.ServicioAdicional.Commands.Update;
using DcodePe.Catering.Domain.Entities.Clientes;

namespace DcodePe.Catering.Application.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<UserEntity, CreateUserModel>().ReverseMap();
            CreateMap<UserEntity, UpdateUserModel>().ReverseMap();

            CreateMap<UserEntity, GetAllUserModel>().ReverseMap();
            CreateMap<UserEntity, GetUserByIdModel>().ReverseMap();
            CreateMap<UserEntity, GetUserByUserNameAndPasswordModel>().ReverseMap();


            CreateMap<CustomerEntity, CreateCustomerModel>().ReverseMap();
            CreateMap<CustomerEntity, UpdateCustomerModel>().ReverseMap();

            CreateMap<CustomerEntity, GetAllCustomerModel>().ReverseMap();
            CreateMap<CustomerEntity, GetCustomerByIdModel>().ReverseMap();
            CreateMap<CustomerEntity, GetCustomerByDocumentNumberModel>().ReverseMap();

            CreateMap<BookingEntity, CreateBookingModel>().ReverseMap();
            CreateMap<BookingEntity, UpdateBookingModel>().ReverseMap();

            // Mappings para Local (CRUD completo)
            CreateMap<LocalEntity, CreateLocalModel>().ReverseMap();
            CreateMap<LocalEntity, UpdateLocalModel>().ReverseMap();
            CreateMap<LocalEntity, GetAllLocalModel>().ReverseMap();

            // Mappings para Evento (CRUD completo)
            CreateMap<EventoEntity, CreateEventoModel>().ReverseMap();
            CreateMap<EventoEntity, UpdateEventoModel>().ReverseMap();
            CreateMap<EventoEntity, GetAllEventoModel>().ReverseMap();

            CreateMap<UsuarioEntity, GetAllUsuarioModel>().ReverseMap();

            // Mappings para Cliente (CRUD completo)
            CreateMap<ClienteEntity, CreateClienteModel>().ReverseMap();
            CreateMap<ClienteEntity, UpdateClienteModel>().ReverseMap();
            CreateMap<ClienteEntity, GetAllClienteModel>()
                .ForMember(dest => dest.TotalCotizaciones,
                    opt => opt.MapFrom(src => src.Cotizaciones.Count(c => c.Estado == true)));

            CreateMap<ProductoEntity, GetAllProductoModel>().ReverseMap();
            CreateMap<ProductoEntity, CreateProductoModel>().ReverseMap();
            CreateMap<ProductoEntity, UpdateProductoModel>().ReverseMap();
            CreateMap<PaqueteEntity, CreatePaqueteModel>().ReverseMap();
            CreateMap<PaqueteEntity, GetAllPaqueteModel>().ReverseMap();
            CreateMap<PaqueteProductoEntity, CreatePaqueteProductoModel>().ReverseMap();

            // Mapping para Cotizacion - Ignorar colecciones anidadas que se manejan manualmente
            CreateMap<CotizacionEntity, CreateCotizacionModel>()
                .ForMember(dest => dest.CotizacionProducto, opt => opt.Ignore())
                .ForMember(dest => dest.CotizacionServicio, opt => opt.Ignore())
                //.ForMember(dest => dest.CotizacionPaquete, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.CotizacionProducto, opt => opt.Ignore())
                .ForMember(dest => dest.CotizacionServicio, opt => opt.Ignore());
            //.ForMember(dest => dest.CotizacionPaquete, opt => opt.Ignore());

            CreateMap<CotizacionProductoEntity, CreateCotizacionProductoModel>().ReverseMap();
            CreateMap<CotizacionServicioEntity, CreateCotizacionServicioModel>().ReverseMap();
            CreateMap<CotizacionPaqueteEntity, CreateCotizacionPaqueteModel>().ReverseMap();

            CreateMap<ServicioAdicionalEntity, CreateServicioAdicionalModel>().ReverseMap();
            CreateMap<ServicioAdicionalEntity, UpdateServicioAdicionalModel>().ReverseMap();
            CreateMap<PaqueteServicioEntity, CreatePaqueteServicioModel>().ReverseMap();

            // Mappings para Categoria (CRUD completo)
            CreateMap<CategoriaEntity, CreateCategoriaModel>().ReverseMap();
            CreateMap<CategoriaEntity, CreateCategoriaResponseModel>().ReverseMap();
            CreateMap<CategoriaEntity, UpdateCategoriaModel>().ReverseMap();
            CreateMap<CategoriaEntity, GetAllCategoriaModel>()
                .ForMember(dest => dest.TotalProductos,
                    opt => opt.MapFrom(src => src.Productos.Count(p => p.Estado == true)));
            CreateMap<CategoriaEntity, GetCategoriaByIdModel>()
                .ForMember(dest => dest.TotalProductos,
                    opt => opt.MapFrom(src => src.Productos.Count(p => p.Estado == true)));

            CreateMap<ContactanosEntity, CreateContactanosModel>().ReverseMap();
            CreateMap<ContactanosEntity, GetAllContactanosModel>().ReverseMap();

            // Mappings para Rol (CRUD completo)
            CreateMap<RolEntity, CreateRolModel>().ReverseMap();
            CreateMap<RolEntity, UpdateRolModel>().ReverseMap();
            CreateMap<RolEntity, GetAllRolModel>().ReverseMap();
            CreateMap<RolEntity, GetRolByIdModel>().ReverseMap();

            // Mappings para Pagina (CRUD completo)
            CreateMap<PaginaEntity, CreatePaginaModel>().ReverseMap();
            CreateMap<PaginaEntity, UpdatePaginaModel>().ReverseMap();
            CreateMap<PaginaEntity, GetAllPaginaModel>().ReverseMap();

            // Mappings para Permiso (CRUD completo)
            CreateMap<PermisoEntity, CreatePermisoModel>().ReverseMap();
            CreateMap<PermisoEntity, UpdatePermisoModel>().ReverseMap();
        }
    }
}
