namespace DcodePe.Catering.Application.DataBase.Permiso.Commands.Update
{
    public class UpdatePermisoCommand : IUpdatePermisoCommand
    {
        private readonly IDataBaseService _databaseService;

        public UpdatePermisoCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(UpdatePermisoModel model)
        {
            var entity = await _databaseService.Permiso
                .FirstOrDefaultAsync(p => p.PermisoID == model.PermisoID && p.Estado == true);

            if (entity == null)
                return false;

            entity.PuedeVer = model.PuedeVer;
            entity.PuedeCrear = model.PuedeCrear;
            entity.PuedeEditar = model.PuedeEditar;
            entity.PuedeEliminar = model.PuedeEliminar;
            entity.UsuarioModificacion = model.UsuarioModificacion;
            entity.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
