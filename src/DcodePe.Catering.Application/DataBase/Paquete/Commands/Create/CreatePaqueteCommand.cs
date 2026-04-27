namespace DcodePe.Catering.Application.DataBase.Paquete.Commands.Create
{
    public class CreatePaqueteCommand(IDataBaseService databaseService, IMapper mapper) : ICreatePaqueteCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;
        private readonly IMapper _mapper = mapper;

        public async Task<CreatePaqueteModel> ExecuteSavePaquete(CreatePaqueteModel model)
        {
            var entity = _mapper.Map<PaqueteEntity>(model);
            entity.Estado = true;
            entity.FechaCreacion = DateTime.Now;
            await _databaseService.Paquete.AddAsync(entity);
            await _databaseService.SaveAsync();
            return model;
        }
    }
    
}
