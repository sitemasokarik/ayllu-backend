namespace DcodePe.Catering.Application.DataBase.PaqueteProducto.Commands.Create
{
    public class CreatePaqueteProductoCommand(IDataBaseService databaseService, IMapper mapper) : ICreatePaqueteProductoCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;
        private readonly IMapper _mapper = mapper;
        public async Task<CreatePaqueteProductoModel> ExecuteSavePaqueteProducto(CreatePaqueteProductoModel model)
        {
            var entity = _mapper.Map<PaqueteProductoEntity>(model);
            entity.FechaCreacion = DateTime.Now;
            await _databaseService.PaqueteProducto.AddAsync(entity);
            await _databaseService.SaveAsync();
            return model;
        }
    }
}
