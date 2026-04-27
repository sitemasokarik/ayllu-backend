namespace DcodePe.Catering.Application.DataBase.Producto.Commands.Create
{
    public class CreateProductoCommand(IDataBaseService databaseService, IMapper mapper) : ICreateProductoCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;
        private readonly IMapper _mapper = mapper;
        public async Task<CreateProductoModel> ExecuteSaveProducto(CreateProductoModel model)
        {
            var entity = _mapper.Map<ProductoEntity>(model);
            entity.Estado = true;
            entity.FechaCreacion = DateTime.Now;
            await _databaseService.Producto.AddAsync(entity);
            await _databaseService.SaveAsync();
            return model;
        }
    }
}
