using DcodePe.Catering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Local.Commands.CreateLocal
{
    public class CreateLocalCommand(IDataBaseService databaseService, IMapper mapper) : ICreateLocalCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;
        private readonly IMapper _mapper = mapper;

        public async Task<CreateLocalModel> ExecuteSaveLocal(CreateLocalModel model)
        {
            var entity = _mapper.Map<LocalEntity>(model);
            entity.Estado = true;
            entity.FechaCreacion=DateTime.Now;
            await _databaseService.Local.AddAsync(entity);
            await _databaseService.SaveAsync();
            return model;

        }
    }
}
