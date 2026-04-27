namespace DcodePe.Catering.Application.DataBase.Evento.Commands.Create
{
    public class CreateEventoCommand(IDataBaseService databaseService, IMapper mapper) : ICreateEventoCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;
        private readonly IMapper _mapper = mapper;


        public async Task<CreateEventoModel> ExecuteSaveEvento(CreateEventoModel model)
        {
            var entity = _mapper.Map<EventoEntity>(model);
            entity.Estado = true;
            entity.FechaCreacion = DateTime.Now;
            await _databaseService.Evento.AddAsync(entity);
            await _databaseService.SaveAsync();
            return model;
        }

    }
}
