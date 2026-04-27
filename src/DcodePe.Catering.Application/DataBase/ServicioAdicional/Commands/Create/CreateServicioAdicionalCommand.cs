using AutoMapper;
using DcodePe.Catering.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.ServicioAdicional.Commands.Create
{
    public class CreateServicioAdicionalCommand : ICreateServicioAdicionalCommand
    {
        private readonly IDataBaseService _databaseService;
        private readonly IMapper _mapper;

        public CreateServicioAdicionalCommand(IDataBaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        public async Task<CreateServicioAdicionalModel> ExecuteSaveServicioAdicional(CreateServicioAdicionalModel model)
        {
            var entity = _mapper.Map<ServicioAdicionalEntity>(model);
            
            entity.UsuarioCreacion = model.UsuarioCreacion ?? "SYSTEM";
            entity.FechaCreacion = DateTime.Now;
            entity.Estado = model.Estado ?? true;

            await _databaseService.ServicioAdicional.AddAsync(entity);
            await _databaseService.SaveAsync();

            model.ServicioID = entity.ServicioID;
            return model;
        }
    }
}
