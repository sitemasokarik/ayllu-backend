using AutoMapper;
using DcodePe.Catering.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Contactanos.Commands.Create
{
    public class CreateContactanosCommand : ICreateContactanosCommand
    {
        private readonly IDataBaseService _databaseService;
        private readonly IMapper _mapper;

        public CreateContactanosCommand(IDataBaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        public async Task<CreateContactanosModel> Execute(CreateContactanosModel model)
        {
            var entity = _mapper.Map<ContactanosEntity>(model);
            
            entity.UsuarioCreacion = model.UsuarioCreacion ?? "WEB";
            entity.FechaCreacion = DateTime.Now;
            entity.Estado = model.Estado ?? true;

            await _databaseService.Contactanos.AddAsync(entity);
            await _databaseService.SaveAsync();

            model.ContactanosID = entity.ContactanosID;
            return model;
        }
    }
}
