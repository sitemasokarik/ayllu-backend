namespace DcodePe.Catering.Application.DataBase.Permiso.Commands.Delete
{
    public class DeletePermisoCommand : IDeletePermisoCommand
    {
        private readonly IDataBaseService _databaseService;

        public DeletePermisoCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(int permisoId, string usuarioEliminacion)
        {
            var entity = await _databaseService.Permiso
                .FirstOrDefaultAsync(p => p.PermisoID == permisoId && p.Estado == true);

            if (entity == null)
                return false;

            // Soft Delete
            entity.Estado = false;
            entity.UsuarioEliminacion = usuarioEliminacion;
            entity.FechaEliminacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
