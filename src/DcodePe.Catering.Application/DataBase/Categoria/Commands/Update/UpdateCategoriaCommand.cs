using DcodePe.Catering.Application.DataBase.Categoria.Queries.GetAll;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Categoria.Commands.Update
{
    public class UpdateCategoriaCommand : IUpdateCategoriaCommand
    {
        private readonly IDataBaseService _databaseService;
        private readonly IMapper _mapper;
        private readonly IGetAllCategoriaQuery _getAllCategoriaQuery;

        public UpdateCategoriaCommand(
            IDataBaseService databaseService, 
            IMapper mapper,
            IGetAllCategoriaQuery getAllCategoriaQuery)
        {
            _databaseService = databaseService;
            _mapper = mapper;
            _getAllCategoriaQuery = getAllCategoriaQuery;
        }

        public async Task<bool> Execute(UpdateCategoriaModel model)
        {
            var entity = await _databaseService.Categoria
                .FirstOrDefaultAsync(c => c.CategoriaID == model.CategoriaID);

            if (entity == null)
                return false;

            // Validar que no se cree un círculo en la jerarquía
            if (model.CategoriaPadreID.HasValue)
            {
                var esValido = await _getAllCategoriaQuery.ValidateHierarchyWithoutCircles(
                    model.CategoriaID, 
                    model.CategoriaPadreID);

                if (!esValido)
                    throw new InvalidOperationException(
                        "No se puede establecer esta categoría como padre porque crearía una referencia circular.");
            }

            // Mapear propiedades
            entity.CategoriaPadreID = model.CategoriaPadreID;
            entity.Nombre = model.Nombre;
            entity.Descripcion = model.Descripcion;
            entity.Nivel = model.Nivel;
            entity.Orden = model.Orden;
            entity.Icono = model.Icono;
            entity.Limite = model.Limite;
            entity.UsuarioModificacion = model.UsuarioModificacion;
            entity.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
