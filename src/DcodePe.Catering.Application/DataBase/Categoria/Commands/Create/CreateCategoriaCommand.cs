using DcodePe.Catering.Domain.Entities;
using AutoMapper;

namespace DcodePe.Catering.Application.DataBase.Categoria.Commands.Create
{
    public class CreateCategoriaCommand : ICreateCategoriaCommand
    {
        private readonly IDataBaseService _databaseService;
        private readonly IMapper _mapper;

        public CreateCategoriaCommand(IDataBaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        public async Task<CreateCategoriaResponseModel> Execute(CreateCategoriaModel model)
        {
            var entity = _mapper.Map<CategoriaEntity>(model);
            entity.FechaCreacion = DateTime.Now;
            entity.Estado = true;

            await _databaseService.Categoria.AddAsync(entity);
            await _databaseService.SaveAsync();

            return _mapper.Map<CreateCategoriaResponseModel>(entity);
        }
    }
}
