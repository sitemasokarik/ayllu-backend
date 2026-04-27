using DcodePe.Catering.Domain.Entities;
using DcodePe.Catering.Application.Security;

namespace DcodePe.Catering.Application.DataBase.Usuario.Commands.Create
{
    public class CreateUsuarioCommand : ICreateUsuarioCommand
    {
        private readonly IDataBaseService _databaseService;
        private readonly IPasswordHashService _passwordHashService;

        public CreateUsuarioCommand(
            IDataBaseService databaseService,
            IPasswordHashService passwordHashService)
        {
            _databaseService = databaseService;
            _passwordHashService = passwordHashService;
        }

        public async Task<CreateUsuarioResponseModel> Execute(CreateUsuarioModel model)
        {
            // Crear entidad con contraseña hasheada
            var entity = new UsuarioEntity
            {
                Nombre = model.Nombre,
                UserName = model.UserName,
                Email = model.Email,
                Password = _passwordHashService.HashPassword(model.Password), // Hash con BCrypt
                RolID = model.RolID,
                UsuarioCreacion = model.UsuarioCreacion ?? "SYSTEM",
                FechaCreacion = DateTime.Now,
                Estado = true
            };

            await _databaseService.Usuario.AddAsync(entity);
            await _databaseService.SaveAsync();

            return new CreateUsuarioResponseModel
            {
                UsuarioID = entity.UsuarioID,
                Nombre = entity.Nombre,
                UserName = entity.UserName,
                Email = entity.Email,
                RolID = entity.RolID,
                FechaCreacion = entity.FechaCreacion
            };
        }
    }
}
