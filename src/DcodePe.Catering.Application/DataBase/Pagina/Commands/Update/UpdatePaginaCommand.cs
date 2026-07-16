namespace DcodePe.Catering.Application.DataBase.Pagina.Commands.Update
{
    public class UpdatePaginaCommand : IUpdatePaginaCommand
    {
        private readonly IDataBaseService _databaseService;

        public UpdatePaginaCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(UpdatePaginaModel model)
        {
            var entity = await _databaseService.Pagina
                .FirstOrDefaultAsync(p => p.PaginaID == model.PaginaID && p.Estado == true);

            if (entity == null)
                return false;

            // Validar nombre �nico (excepto el actual)
            var existePagina = await _databaseService.Pagina
                .AnyAsync(p => p.Nombre == model.Nombre &&
                              p.PaginaID != model.PaginaID &&
                              p.Estado == true);

            if (existePagina)
                throw new InvalidOperationException($"Ya existe otra p�gina con el nombre '{model.Nombre}'");

            entity.Nombre = model.Nombre;
            entity.Descripcion = model.Descripcion;
            entity.Url = model.Url;
            entity.Icono = model.Icono;
            entity.GrupoMenu = model.GrupoMenu;
            entity.OrdenMenu = model.OrdenMenu;
            entity.UsuarioModificacion = model.UsuarioModificacion;
            entity.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
