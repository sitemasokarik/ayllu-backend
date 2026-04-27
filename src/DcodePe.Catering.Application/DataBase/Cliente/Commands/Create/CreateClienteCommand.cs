using DcodePe.Catering.Domain.Entities.Clientes;

namespace DcodePe.Catering.Application.DataBase.Cliente.Commands.Create
{
    public class CreateClienteCommand : ICreateClienteCommand
    {
        private readonly IDataBaseService _databaseService;
        private readonly IMapper _mapper;

        public CreateClienteCommand(IDataBaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        public async Task<CreateClienteModel> Execute(CreateClienteModel model)
        {
            var entity = _mapper.Map<ClienteEntity>(model);
            entity.Estado = true;
            entity.FechaCreacion = DateTime.Now;

            await _databaseService.Cliente.AddAsync(entity);
            await _databaseService.SaveAsync();
            model.ClienteID = entity.ClienteID;

            return model;
        }
    }
}
