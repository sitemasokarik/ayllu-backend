using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Empresa.Queries.GetAllEmpresa
{
    public class GetAllEmpresaQuery : IGetAllEmpresaQuery
    {
        private readonly IDataBaseService _databaseService;

        public GetAllEmpresaQuery(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<GetAllEmpresaModel>> ExecuteListEmpresa()
        {
            var result = await _databaseService.Empresa
                .Where(e => e.Estado == true)
                .Select(empresa => new GetAllEmpresaModel
                {
                    EmpresaID = empresa.EmpresaID,
                    RazonSocial = empresa.RazonSocial,
                    NombreComercial = empresa.NombreComercial,
                    RUC = empresa.RUC,
                    Email = empresa.Email,
                    Telefono = empresa.Telefono,
                    TelefonoSecundario = empresa.TelefonoSecundario,
                    WhatsApp = empresa.WhatsApp,
                    Direccion = empresa.Direccion,
                    Ciudad = empresa.Ciudad,
                    Pais = empresa.Pais,
                    Facebook = empresa.Facebook,
                    Instagram = empresa.Instagram,
                    LinkedIn = empresa.LinkedIn,
                    Twitter = empresa.Twitter,
                    HorarioAtencion = empresa.HorarioAtencion,
                    Logo = empresa.Logo,
                    UsuarioCreacion = empresa.UsuarioCreacion,
                    FechaCreacion = empresa.FechaCreacion,
                    UsuarioModificacion = empresa.UsuarioModificacion,
                    FechaModificacion = empresa.FechaModificacion,
                    UsuarioEliminacion = empresa.UsuarioEliminacion,
                    FechaEliminacion = empresa.FechaEliminacion,
                    Estado = empresa.Estado
                })
                .ToListAsync();

            return result;
        }

        public async Task<GetAllEmpresaModel> ExecuteGetEmpresaById(int empresaId)
        {
            var empresa = await _databaseService.Empresa
                .Where(e => e.EmpresaID == empresaId && e.Estado==true)
                .FirstOrDefaultAsync();

            if (empresa == null)
                return null;

            return new GetAllEmpresaModel
            {
                EmpresaID = empresa.EmpresaID,
                RazonSocial = empresa.RazonSocial,
                NombreComercial = empresa.NombreComercial,
                RUC = empresa.RUC,
                Email = empresa.Email,
                Telefono = empresa.Telefono,
                TelefonoSecundario = empresa.TelefonoSecundario,
                WhatsApp = empresa.WhatsApp,
                Direccion = empresa.Direccion,
                Ciudad = empresa.Ciudad,
                Pais = empresa.Pais,
                Facebook = empresa.Facebook,
                Instagram = empresa.Instagram,
                LinkedIn = empresa.LinkedIn,
                Twitter = empresa.Twitter,
                HorarioAtencion = empresa.HorarioAtencion,
                Logo = empresa.Logo,
                UsuarioCreacion = empresa.UsuarioCreacion,
                FechaCreacion = empresa.FechaCreacion,
                UsuarioModificacion = empresa.UsuarioModificacion,
                FechaModificacion = empresa.FechaModificacion,
                UsuarioEliminacion = empresa.UsuarioEliminacion,
                FechaEliminacion = empresa.FechaEliminacion,
                Estado = empresa.Estado
            };
        }

        public async Task<GetAllEmpresaModel> ExecuteGetEmpresaActiva()
        {
            var empresa = await _databaseService.Empresa
                .Where(e => e.Estado == true)
                .OrderBy(e => e.EmpresaID)
                .FirstOrDefaultAsync();

            if (empresa == null)
                return null;

            return new GetAllEmpresaModel
            {
                EmpresaID = empresa.EmpresaID,
                RazonSocial = empresa.RazonSocial,
                NombreComercial = empresa.NombreComercial,
                RUC = empresa.RUC,
                Email = empresa.Email,
                Telefono = empresa.Telefono,
                TelefonoSecundario = empresa.TelefonoSecundario,
                WhatsApp = empresa.WhatsApp,
                Direccion = empresa.Direccion,
                Ciudad = empresa.Ciudad,
                Pais = empresa.Pais,
                Facebook = empresa.Facebook,
                Instagram = empresa.Instagram,
                LinkedIn = empresa.LinkedIn,
                Twitter = empresa.Twitter,
                HorarioAtencion = empresa.HorarioAtencion,
                Logo = empresa.Logo,
                UsuarioCreacion = empresa.UsuarioCreacion,
                FechaCreacion = empresa.FechaCreacion,
                UsuarioModificacion = empresa.UsuarioModificacion,
                FechaModificacion = empresa.FechaModificacion,
                UsuarioEliminacion = empresa.UsuarioEliminacion,
                FechaEliminacion = empresa.FechaEliminacion,
                Estado = empresa.Estado
            };
        }
    }
}
