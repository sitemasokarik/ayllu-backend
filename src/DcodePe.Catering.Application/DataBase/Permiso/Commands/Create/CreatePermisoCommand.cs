using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Permiso.Commands.Create
{
    public class CreatePermisoCommand : ICreatePermisoCommand
    {
        private readonly IDataBaseService _databaseService;

        public CreatePermisoCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<CreatePermisoModel> Execute(CreatePermisoModel model)
        {
            // Validar que no exista el permiso para ese Rol-Pagina
            var existePermiso = await _databaseService.Permiso
                .AnyAsync(p => p.RolID == model.RolID && 
                              p.PaginaID == model.PaginaID && 
                              p.Estado == true);

            if (existePermiso)
                throw new InvalidOperationException("Ya existe un permiso para este rol y página");

            var entity = new Domain.Entities.PermisoEntity
            {
                RolID = model.RolID,
                PaginaID = model.PaginaID,
                PuedeVer = model.PuedeVer,
                PuedeCrear = model.PuedeCrear,
                PuedeEditar = model.PuedeEditar,
                PuedeEliminar = model.PuedeEliminar,
                UsuarioCreacion = model.UsuarioCreacion,
                FechaCreacion = DateTime.Now,
                Estado = true
            };

            await _databaseService.Permiso.AddAsync(entity);
            await _databaseService.SaveAsync();

            return model;
        }
    }
}
