namespace DcodePe.Catering.Application.DataBase.Pagina.Commands.Create
{
    public class CreatePaginaCommand : ICreatePaginaCommand
    {
        private readonly IDataBaseService _databaseService;

        public CreatePaginaCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<CreatePaginaModel> Execute(CreatePaginaModel model)
        {
            var existePagina = await _databaseService.Pagina
                .AnyAsync(p => p.Nombre == model.Nombre && p.Estado == true);

            if (existePagina)
                throw new InvalidOperationException($"Ya existe una página con el nombre '{model.Nombre}'");

            var entity = new Domain.Entities.PaginaEntity
            {
                Nombre = model.Nombre,
                Descripcion = model.Descripcion,
                Url = model.Url,
                Icono = model.Icono,
                UsuarioCreacion = model.UsuarioCreacion,
                FechaCreacion = DateTime.Now,
                Estado = true
            };

            await _databaseService.Pagina.AddAsync(entity);
            await _databaseService.SaveAsync();

            return model;
        }
    }
}
