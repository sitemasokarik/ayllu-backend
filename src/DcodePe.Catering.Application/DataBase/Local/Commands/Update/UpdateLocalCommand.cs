using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Local.Commands.Update
{
    public class UpdateLocalCommand : IUpdateLocalCommand
    {
        private readonly IDataBaseService _databaseService;
        private readonly IMapper _mapper;

        public UpdateLocalCommand(IDataBaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        public async Task<bool> Execute(UpdateLocalModel model)
        {
            var entity = await _databaseService.Local
                .FirstOrDefaultAsync(l => l.LocalID == model.LocalID);

            if (entity == null)
                return false;

            _mapper.Map(model, entity);
            entity.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
