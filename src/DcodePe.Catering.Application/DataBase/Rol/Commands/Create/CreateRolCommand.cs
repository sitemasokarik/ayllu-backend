namespace DcodePe.Catering.Application.DataBase.Rol.Commands.Create
{
    public class CreateRolCommand : ICreateRolCommand
    {
        private readonly IDataBaseService _databaseService;
        private readonly IMapper _mapper;

        public CreateRolCommand(IDataBaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        public async Task<CreateRolModel> Execute(CreateRolModel model)
        {
            // Validar que no exista un rol con el mismo nombre
            var existeRol = await _databaseService.Rol
                .AnyAsync(r => r.Nombre == model.Nombre && r.Estado == true);

            if (existeRol)
                throw new InvalidOperationException($"Ya existe un rol con el nombre '{model.Nombre}'");

            var entity = _mapper.Map<RolEntity>(model);
            entity.FechaCreacion = DateTime.Now;
            entity.Estado = true;

            await _databaseService.Rol.AddAsync(entity);
            await _databaseService.SaveAsync();

            model.UsuarioCreacion = entity.UsuarioCreacion;
            return model;
        }
    }
}
