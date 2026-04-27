using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Usuario.Queries.GetUsuarioById
{
    public class GetUsuarioByIdQuery : IGetUsuarioByIdQuery
    {
        private readonly IDataBaseService _databaseService;

        public GetUsuarioByIdQuery(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<GetUsuarioByIdModel> Execute(int usuarioId)
        {
            var usuario = await _databaseService.Usuario.Where(u => u.UsuarioID == usuarioId && u.Rol.Estado==true)
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.UsuarioID == usuarioId);

            if (usuario == null)
                return null;

            return new GetUsuarioByIdModel
            {
                UsuarioID = usuario.UsuarioID,
                Nombre = usuario.Nombre,
                UserName = usuario.UserName,
                Email = usuario.Email,
                RolID = usuario.RolID,
                RolNombre = usuario.Rol?.Nombre,
                UsuarioCreacion = usuario.UsuarioCreacion,
                FechaCreacion = usuario.FechaCreacion,
                UsuarioModificacion = usuario.UsuarioModificacion,
                FechaModificacion = usuario.FechaModificacion,
                Estado = usuario.Estado
            };
        }
    }
}
