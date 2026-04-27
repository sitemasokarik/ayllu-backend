using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Empresa.Commands.Update
{
    public class UpdateEmpresaCommand : IUpdateEmpresaCommand
    {
        private readonly IDataBaseService _databaseService;

        public UpdateEmpresaCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(UpdateEmpresaModel model)
        {
            var entity = await _databaseService.Empresa
                .FirstOrDefaultAsync(e => e.EmpresaID == model.EmpresaID);

            if (entity == null)
                return false;

            entity.RazonSocial = model.RazonSocial;
            entity.NombreComercial = model.NombreComercial;
            entity.RUC = model.RUC;
            entity.Email = model.Email;
            entity.Telefono = model.Telefono;
            entity.TelefonoSecundario = model.TelefonoSecundario;
            entity.WhatsApp = model.WhatsApp;
            entity.Direccion = model.Direccion;
            entity.Ciudad = model.Ciudad;
            entity.Pais = model.Pais;
            entity.Facebook = model.Facebook;
            entity.Instagram = model.Instagram;
            entity.LinkedIn = model.LinkedIn;
            entity.Twitter = model.Twitter;
            entity.HorarioAtencion = model.HorarioAtencion;
            entity.Logo = model.Logo;
            entity.UsuarioModificacion = model.UsuarioModificacion ?? "SYSTEM";
            entity.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
