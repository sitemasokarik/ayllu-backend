using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Evento.Commands.Update
{
    public class UpdateEventoCommand : IUpdateEventoCommand
    {
        private readonly IDataBaseService _databaseService;
        private readonly IMapper _mapper;

        public UpdateEventoCommand(IDataBaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        public async Task<bool> Execute(UpdateEventoModel model)
        {
            var entity = await _databaseService.Evento
                .FirstOrDefaultAsync(e => e.EventoID == model.EventoID);

            if (entity == null)
                return false;

            _mapper.Map(model, entity);
            entity.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
