using Microsoft.EntityFrameworkCore;
using DcodePe.Catering.Application.Security;

namespace DcodePe.Catering.Application.DataBase.Usuario.Queries.GetUsuarioByCredentials
{
    public class GetUsuarioByCredentialsQuery : IGetUsuarioByCredentialsQuery
    {
        private readonly IDataBaseService _dataBaseService;
        private readonly IPasswordHashService _passwordHashService;

        public GetUsuarioByCredentialsQuery(
            IDataBaseService dataBaseService,
            IPasswordHashService passwordHashService)
        {
            _dataBaseService = dataBaseService;
            _passwordHashService = passwordHashService;
        }

        public async Task<GetUsuarioByCredentialsModel> Execute(string userName, string password)
        {
            // Buscar usuario por UserName o Email
            var usuario = await _dataBaseService.Usuario
                .Where(x => x.Estado == true && x.Rol.Estado == true)
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(x => 
                    (x.UserName == userName || x.Email == userName) &&
                    x.Estado == true);

            if (usuario == null)
                return null;

            // Verificar contraseña con BCrypt
            bool isPasswordValid = _passwordHashService.VerifyPassword(password, usuario.Password);

            if (!isPasswordValid)
                return null;

            return new GetUsuarioByCredentialsModel
            {
                UsuarioID = usuario.UsuarioID,
                Nombre = usuario.Nombre,
                UserName = usuario.UserName,
                Email = usuario.Email,
                RolID = usuario.RolID,
                RolNombre = usuario.Rol?.Nombre,
                //FechaCreacion = usuario.FechaCreacion
            };
        }
    }
}
