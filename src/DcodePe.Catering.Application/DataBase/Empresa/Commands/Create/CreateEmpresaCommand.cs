using DcodePe.Catering.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Empresa.Commands.Create
{
    public class CreateEmpresaCommand : ICreateEmpresaCommand
    {
        private readonly IDataBaseService _databaseService;

        public CreateEmpresaCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<CreateEmpresaModel> Execute(CreateEmpresaModel model)
        {
            var entity = new EmpresaEntity
            {
                RazonSocial = model.RazonSocial,
                NombreComercial = model.NombreComercial,
                RUC = model.RUC,
                Email = model.Email,
                Telefono = model.Telefono,
                TelefonoSecundario = model.TelefonoSecundario,
                WhatsApp = model.WhatsApp,
                Direccion = model.Direccion,
                Ciudad = model.Ciudad,
                Pais = model.Pais ?? "Perú",
                Facebook = model.Facebook,
                Instagram = model.Instagram,
                LinkedIn = model.LinkedIn,
                Twitter = model.Twitter,
                HorarioAtencion = model.HorarioAtencion,
                Logo = model.Logo,
                UsuarioCreacion = model.UsuarioCreacion ?? "SYSTEM",
                FechaCreacion = DateTime.Now,
                Estado = model.Estado ?? true
            };

            await _databaseService.Empresa.AddAsync(entity);
            await _databaseService.SaveAsync();

            model.EmpresaID = entity.EmpresaID;
            return model;
        }
    }
}
