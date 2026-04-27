using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Producto.Commands.Update
{
    public class UpdateProductoCommand : IUpdateProductoCommand
    {
        private readonly IDataBaseService _databaseService;
        private readonly IMapper _mapper;

        public UpdateProductoCommand(IDataBaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        public async Task<bool> Execute(UpdateProductoModel model)
        {
            var entity = await _databaseService.Producto
                .FirstOrDefaultAsync(p => p.ProductoID == model.ProductoID);

            if (entity == null)
                return false;

            _mapper.Map(model, entity);
            entity.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
