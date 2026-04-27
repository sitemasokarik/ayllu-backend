using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.ServicioAdicional.Commands.Update
{
    public class UpdateServicioAdicionalCommand : IUpdateServicioAdicionalCommand
    {
        private readonly IDataBaseService _databaseService;
        private readonly IMapper _mapper;

        public UpdateServicioAdicionalCommand(IDataBaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        public async Task<bool> Execute(UpdateServicioAdicionalModel model)
        {
            var entity = await _databaseService.ServicioAdicional
                .FirstOrDefaultAsync(s => s.ServicioID == model.ServicioID);

            if (entity == null)
                return false;

            _mapper.Map(model, entity);
            
            entity.UsuarioModificacion = model.UsuarioModificacion ?? "SYSTEM";
            entity.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
