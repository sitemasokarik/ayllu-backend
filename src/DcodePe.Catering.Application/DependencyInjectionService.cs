using DcodePe.Catering.Application.DataBase.Blog.Commands.Create;
using DcodePe.Catering.Application.DataBase.Blog.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Blog.Commands.Update;
using DcodePe.Catering.Application.DataBase.Blog.Queries.GetAllBlog;
using DcodePe.Catering.Application.DataBase.Categoria.Commands.Create;
using DcodePe.Catering.Application.DataBase.Categoria.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Categoria.Commands.Update;
using DcodePe.Catering.Application.DataBase.Categoria.Queries.GetAll;
using DcodePe.Catering.Application.DataBase.Categoria.Queries.GetById;
using DcodePe.Catering.Application.DataBase.Cliente.Commands.Create;
using DcodePe.Catering.Application.DataBase.Cliente.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Cliente.Commands.Update;
using DcodePe.Catering.Application.DataBase.Cliente.Queries.GetAllCliente;
using DcodePe.Catering.Application.DataBase.Contactanos.Commands.Create;
using DcodePe.Catering.Application.DataBase.Contactanos.Queries.GetAllContactanos;
using DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Update;
using DcodePe.Catering.Application.DataBase.Empresa.Commands.Create;
using DcodePe.Catering.Application.DataBase.Empresa.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Empresa.Commands.Update;
using DcodePe.Catering.Application.DataBase.Empresa.Queries.GetAllEmpresa;
using DcodePe.Catering.Application.DataBase.Evento.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Evento.Commands.Update;
using DcodePe.Catering.Application.DataBase.Evento.Queries.GetById;
using DcodePe.Catering.Application.DataBase.Local.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Local.Commands.Update;
using DcodePe.Catering.Application.DataBase.Pagina.Commands.Create;
using DcodePe.Catering.Application.DataBase.Pagina.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Pagina.Commands.Update;
using DcodePe.Catering.Application.DataBase.Pagina.Queries.GetAll;
using DcodePe.Catering.Application.DataBase.Pagina.Queries.GetById;
using DcodePe.Catering.Application.DataBase.PaqueteServicio.Queries.GetAllPaqueteServicio;
using DcodePe.Catering.Application.DataBase.Permiso.Commands.Create;
using DcodePe.Catering.Application.DataBase.Permiso.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Permiso.Commands.Update;
using DcodePe.Catering.Application.DataBase.Permiso.Queries.GetAll;
using DcodePe.Catering.Application.DataBase.Permiso.Queries.GetByRolId;
using DcodePe.Catering.Application.DataBase.Producto.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Producto.Commands.Update;
using DcodePe.Catering.Application.DataBase.Rol.Commands.Create;
using DcodePe.Catering.Application.DataBase.Rol.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Rol.Commands.Update;
using DcodePe.Catering.Application.DataBase.Rol.Queries.GetAll;
using DcodePe.Catering.Application.DataBase.Rol.Queries.GetById;
using DcodePe.Catering.Application.DataBase.ServicioAdicional.Commands.Delete;
using DcodePe.Catering.Application.DataBase.ServicioAdicional.Commands.Update;
using DcodePe.Catering.Application.DataBase.Usuario.Commands.Create;
using DcodePe.Catering.Application.DataBase.Usuario.Commands.Delete;
using DcodePe.Catering.Application.DataBase.Usuario.Commands.Update;
using DcodePe.Catering.Application.DataBase.Usuario.Commands.UpdatePassword;
using DcodePe.Catering.Application.DataBase.Usuario.Queries.GetUsuarioByCredentials;
using DcodePe.Catering.Application.DataBase.Usuario.Queries.GetUsuarioById;
using DcodePe.Catering.Application.Security;
using DcodePe.Catering.Application.Validators.Categoria;
using DcodePe.Catering.Application.Validators.Cliente;
using DcodePe.Catering.Application.Validators.Cotizacion;
using DcodePe.Catering.Application.Validators.Pagina;
using DcodePe.Catering.Application.Validators.Paquete;
using DcodePe.Catering.Application.Validators.Producto;
using DcodePe.Catering.Application.Validators.Rol;
using DcodePe.Catering.Application.Validators.Usuario;

namespace DcodePe.Catering.Application
{
    public static class DependencyInjectionService
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var mapper = new MapperConfiguration(config =>
            {
                config.AddProfile(new MapperProfile());
            });

            services.AddSingleton(mapper.CreateMapper());

            // ? Servicio de hash de contraseñas con BCrypt
            services.AddScoped<IPasswordHashService, PasswordHashService>();

            services.AddTransient<ICreateUserCommand, CreateUserCommand>();
            services.AddTransient<IUpdateUserCommand, UpdateUserCommand>();
            services.AddTransient<IDeleteUserCommand, DeleteUserCommand>();
            services.AddTransient<IUpdateUserPasswordCommand, UpdateUserPasswordCommand>();

            services.AddTransient<IGetAllUserQuery, GetAllUserQuery>();
            services.AddTransient<IGetUserByIdQuery, GetUserByIdQuery>();
            services.AddTransient<IGetUserByUserNameAndPasswordQuery, GetUserByUserNameAndPasswordQuery>();

            services.AddTransient<ICreateCustomerCommand, CreateCustomerCommand>();
            services.AddTransient<IUpdateCustomerCommand, UpdateCustomerCommand>();
            services.AddTransient<IDeleteCustomerCommand, DeleteCustomerCommand>();

            services.AddTransient<IGetAllCustomerQuery, GetAllCustomerQuery>();
            services.AddTransient<IGetCustomerByIdQuery, GetCustomerByIdQuery>();
            services.AddTransient<IGetCustomerByDocumentNumberQuery, GetCustomerByDocumentNumberQuery>();


            services.AddTransient<ICreateBookingCommand, CreateBookingCommand>();
            services.AddTransient<IUpdateBookingCommand, UpdateBookingCommand>();
            services.AddTransient<IDeleteBookingCommand, DeleteBookingCommand>();

            services.AddTransient<IGetAllBookingQuery, GetAllBookingQuery>();
            services.AddTransient<IGetBookingsByDocumentNumberQuery, GetBookingsByDocumentNumberQuery>();
            services.AddTransient<IGetBookingsByTypeQuery, GetBookingsByTypeQuery>();

            services.AddScoped<IValidator<CreateUserModel>, CreateUserValidator>();
            services.AddScoped<IValidator<UpdateUserModel>, UpdateUserValidator>();
            services.AddScoped<IValidator<UpdateUserPasswordModel>, UpdateUserPasswordValidator>();
            services.AddScoped<IValidator<(string, string)>, IGetUserByUserNameAndPasswordValidator>();


            services.AddScoped<IValidator<CreateCustomerModel>, CreateCustomerValidator>();
            services.AddScoped<IValidator<UpdateCustomerModel>, UpdateCustomerValidator>();

            services.AddScoped<IValidator<CreateBookingModel>, CreateBookingValidator>();

            services.AddScoped<IValidator<CreatePaqueteProductosModel>, CreatePaqueteProductosValidator>();

            // ? Validators para Usuario (CRUD completo)
            services.AddScoped<IValidator<(string, string)>, LoginUsuarioValidator>();
            services.AddScoped<IValidator<CreateUsuarioModel>, CreateUsuarioValidator>();
            services.AddScoped<IValidator<UpdateUsuarioModel>, UpdateUsuarioValidator>();
            services.AddScoped<IValidator<DcodePe.Catering.Application.DataBase.Usuario.Commands.UpdatePassword.UpdateUsuarioPasswordModel>, UpdateUsuarioPasswordValidator>();

            // ? Validators para Categoria (CRUD completo)
            services.AddScoped<IValidator<CreateCategoriaModel>, CreateCategoriaValidator>();
            services.AddScoped<IValidator<UpdateCategoriaModel>, UpdateCategoriaValidator>();

            // ? Validators para Producto (CRUD completo)
            services.AddScoped<IValidator<CreateProductoModel>, CreateProductoValidator>();
            services.AddScoped<IValidator<UpdateProductoModel>, UpdateProductoValidator>();

            // ? Validators para Cliente (CRUD completo)
            services.AddScoped<IValidator<CreateClienteModel>, CreateClienteValidator>();
            services.AddScoped<IValidator<UpdateClienteModel>, UpdateClienteValidator>();

            // ? Validators para Rol (CRUD completo)
            services.AddScoped<IValidator<CreateRolModel>, CreateRolValidator>();
            services.AddScoped<IValidator<UpdateRolModel>, UpdateRolValidator>();

            // ? Validators para Cotizacion (CRUD completo)
            services.AddScoped<IValidator<CreateCotizacionModel>, CreateCotizacionValidator>();
            services.AddScoped<IValidator<UpdateCotizacionModel>, UpdateCotizacionValidator>();

            // ? Validators para Pagina (CRUD completo)
            services.AddScoped<IValidator<CreatePaginaModel>, CreatePaginaValidator>();
            services.AddScoped<IValidator<UpdatePaginaModel>, UpdatePaginaValidator>();

            services.AddTransient<ICreateLocalCommand, CreateLocalCommand>();
            services.AddTransient<IUpdateLocalCommand, UpdateLocalCommand>();
            services.AddTransient<IDeleteLocalCommand, DeleteLocalCommand>();
            services.AddTransient<IGetAllLocalQuery, GetAllLocalQuery>();

            services.AddTransient<ICreateEventoCommand, CreateEventoCommand>();
            services.AddTransient<IUpdateEventoCommand, UpdateEventoCommand>();
            services.AddTransient<IDeleteEventoCommand, DeleteEventoCommand>();
            services.AddTransient<IGetAllEventoQuery, GetAllEventoQuery>();
            services.AddTransient<IGetEventoByIdQuery, GetEventoByIdQuery>();
            services.AddTransient<IGetAllEventoUsuarioQuery, GetAllEventoUsuarioQuery>();
            services.AddTransient<IGetAllUsuarioQuery, GetAllUsuarioQuery>();

            // ? Commands para Usuario (CRUD completo)
            services.AddTransient<ICreateUsuarioCommand, CreateUsuarioCommand>();
            services.AddTransient<IUpdateUsuarioCommand, UpdateUsuarioCommand>();
            services.AddTransient<IDeleteUsuarioCommand, DeleteUsuarioCommand>();
            services.AddTransient<IUpdateUsuarioPasswordCommand, UpdateUsuarioPasswordCommand>();

            // ? Queries para Usuario (CRUD completo)
            services.AddTransient<IGetUsuarioByCredentialsQuery, GetUsuarioByCredentialsQuery>();
            services.AddTransient<IGetUsuarioByIdQuery, GetUsuarioByIdQuery>();

            // ? Commands para Categoria (CRUD completo)
            services.AddTransient<ICreateCategoriaCommand, CreateCategoriaCommand>();
            services.AddTransient<IUpdateCategoriaCommand, UpdateCategoriaCommand>();
            services.AddTransient<IDeleteCategoriaCommand, DeleteCategoriaCommand>();

            // ? Queries para Categoria (CRUD completo)
            services.AddTransient<IGetAllCategoriaQuery, GetAllCategoriaQuery>();
            services.AddTransient<IGetCategoriaByIdQuery, GetCategoriaByIdQuery>();

            // ? Cliente - Queries y Commands (CRUD completo)
            services.AddTransient<ICreateClienteCommand, CreateClienteCommand>();
            services.AddTransient<IUpdateClienteCommand, UpdateClienteCommand>();
            services.AddTransient<IDeleteClienteCommand, DeleteClienteCommand>();
            services.AddTransient<IGetAllClienteQuery, GetAllClienteQuery>();

            // ? Producto - Queries y Commands (CRUD completo)
            services.AddTransient<IGetProductoQuery, GetProductoQuery>();
            services.AddScoped<ICreateProductoCommand, CreateProductoCommand>();
            services.AddScoped<IUpdateProductoCommand, UpdateProductoCommand>();
            services.AddScoped<IDeleteProductoCommand, DeleteProductoCommand>();

            services.AddScoped<ICreatePaqueteCommand, CreatePaqueteCommand>();
            services.AddScoped<ICreatePaqueteConProductosCommand, CreatePaqueteConProductosCommand>();
            services.AddScoped<IGetAllPaqueteQuery, GetAllPaqueteQuery>();
            services.AddScoped<ICreatePaqueteProductoCommand, CreatePaqueteProductoCommand>();
            services.AddScoped<ICreateCotizacionCommand, CreateCotizacionCommand>();
            services.AddScoped<IUpdateCotizacionCommand, UpdateCotizacionCommand>();
            services.AddScoped<IDeleteCotizacionCommand, DeleteCotizacionCommand>();
            services.AddScoped<IGetAllCotizacionQuery, GetAllCotizacionQuery>();
            services.AddScoped<ICreateServicioAdicionalCommand, CreateServicioAdicionalCommand>();
            services.AddScoped<IUpdateServicioAdicionalCommand, UpdateServicioAdicionalCommand>();
            services.AddScoped<IDeleteServicioAdicionalCommand, DeleteServicioAdicionalCommand>();
            services.AddScoped<IGetServicioAdicionalQuery, GetServicioAdicionalQuery>();
            services.AddScoped<ICreatePaqueteServicioCommand, CreatePaqueteServicioCommand>();
            services.AddScoped<IGetAllPaqueteServicioQuery, GetAllPaqueteServicioQuery>();

            // Blog / Landing Page
            services.AddScoped<ICreateBlogCommand, CreateBlogCommand>();
            services.AddScoped<IUpdateBlogCommand, UpdateBlogCommand>();
            services.AddScoped<IDeleteBlogCommand, DeleteBlogCommand>();
            services.AddScoped<IGetAllBlogQuery, GetAllBlogQuery>();

            // Empresa
            services.AddScoped<ICreateEmpresaCommand, CreateEmpresaCommand>();
            services.AddScoped<IUpdateEmpresaCommand, UpdateEmpresaCommand>();
            services.AddScoped<IDeleteEmpresaCommand, DeleteEmpresaCommand>();
            services.AddScoped<IGetAllEmpresaQuery, GetAllEmpresaQuery>();

            // Contactanos
            services.AddScoped<ICreateContactanosCommand, CreateContactanosCommand>();
            services.AddScoped<IGetAllContactanosQuery, GetAllContactanosQuery>();

            //Rol - Commands y Queries (CRUD completo)
            services.AddTransient<ICreateRolCommand, CreateRolCommand>();
            services.AddTransient<IUpdateRolCommand, UpdateRolCommand>();
            services.AddTransient<IDeleteRolCommand, DeleteRolCommand>();
            services.AddTransient<IGetAllRolQuery, GetAllRolQuery>();
            services.AddTransient<IGetRolByIdQuery, GetRolByIdQuery>();

            //Pagina - Commands y Queries (CRUD completo)
            services.AddTransient<ICreatePaginaCommand, CreatePaginaCommand>();
            services.AddTransient<IUpdatePaginaCommand, UpdatePaginaCommand>();
            services.AddTransient<IDeletePaginaCommand, DeletePaginaCommand>();
            services.AddTransient<IGetAllPaginaQuery, GetAllPaginaQuery>();
            services.AddTransient<IGetPaginaByIdQuery, GetPaginaByIdQuery>();

            //Permiso - Commands y Queries (CRUD completo)
            services.AddTransient<ICreatePermisoCommand, CreatePermisoCommand>();
            services.AddTransient<IUpdatePermisoCommand, UpdatePermisoCommand>();
            services.AddTransient<IDeletePermisoCommand, DeletePermisoCommand>();
            services.AddTransient<IGetAllPermisoQuery, GetAllPermisoQuery>();
            services.AddTransient<IGetPermisosByRolIdQuery, GetPermisosByRolIdQuery>();

            return services;
        }
    }
}

