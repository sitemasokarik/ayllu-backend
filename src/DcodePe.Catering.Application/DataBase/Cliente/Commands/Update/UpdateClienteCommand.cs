using DcodePe.Catering.Domain.Entities.Clientes;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Cliente.Commands.Update
{
    public class UpdateClienteCommand : IUpdateClienteCommand
    {
        private readonly IDataBaseService _databaseService;
        private readonly IMapper _mapper;

        public UpdateClienteCommand(IDataBaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        public async Task<bool> Execute(UpdateClienteModel model)
        {
            var entity = await _databaseService.Cliente
                .FirstOrDefaultAsync(c => c.ClienteID == model.ClienteID);

            if (entity == null)
                return false;

            _mapper.Map(model, entity);
            entity.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
