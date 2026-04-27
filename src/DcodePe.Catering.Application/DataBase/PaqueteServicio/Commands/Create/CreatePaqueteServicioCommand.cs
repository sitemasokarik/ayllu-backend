
using AutoMapper;
using DcodePe.Catering.Application.Database;

namespace DcodePe.Catering.Application.DataBase.PaqueteServicio.Commands.Create
{
    public class CreatePaqueteServicioCommand(IDataBaseService dataBaseService, IMapper mapper) : ICreatePaqueteServicioCommand
    {
        private readonly IDataBaseService _databaseService = dataBaseService;
        private readonly IMapper _mapper = mapper;
        public async Task<CreatePaqueteServicioModel> CreatePaqueteServicio(CreatePaqueteServicioModel model)
        {

            var entity = _mapper.Map<PaqueteServicioEntity>(model);
            entity.FechaCreacion = DateTime.Now;
            await _databaseService.PaqueteServicio.AddAsync(entity);
            await _databaseService.SaveAsync();
            return model;

        }
    }
}
