using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DcodePe.Catering.Application.DataBase.Empresa.Helpers;

namespace DcodePe.Catering.Application.DataBase.Empresa.Queries.GetAllEmpresa
{
    public class GetAllEmpresaQuery : IGetAllEmpresaQuery
    {
        private readonly IDataBaseService _databaseService;
        private readonly IConfiguration _configuration;

        public GetAllEmpresaQuery(IDataBaseService databaseService, IConfiguration configuration)
        {
            _databaseService = databaseService;
            _configuration = configuration;
        }

        public async Task<List<GetAllEmpresaModel>> ExecuteListEmpresa()
        {
            var empresas = await _databaseService.Empresa
                .Where(e => e.Estado == true)
                .ToListAsync();

            return empresas.Select(Map).ToList();
        }

        public async Task<GetAllEmpresaModel> ExecuteGetEmpresaById(int empresaId)
        {
            var empresa = await _databaseService.Empresa
                .Where(e => e.EmpresaID == empresaId && e.Estado == true)
                .FirstOrDefaultAsync();

            return empresa == null ? null : Map(empresa);
        }

        public async Task<GetAllEmpresaModel> ExecuteGetEmpresaActiva()
        {
            var empresa = await _databaseService.Empresa
                .Where(e => e.Estado == true)
                .OrderBy(e => e.EmpresaID)
                .FirstOrDefaultAsync();

            return empresa == null ? null : Map(empresa);
        }

        public async Task<EmpresaFacturacionConfigModel?> ExecuteGetFacturacionConfig(bool sunatIntegrado)
        {
            var empresa = await _databaseService.Empresa
                .Where(e => e.Estado == true)
                .OrderBy(e => e.EmpresaID)
                .FirstOrDefaultAsync();

            if (empresa == null)
                return null;

            var sunatConfigurado = empresa.GeneraFactElect == true
                && !string.IsNullOrWhiteSpace(empresa.CertificadoFileName)
                && !string.IsNullOrWhiteSpace(empresa.ClaveCertificado)
                && !string.IsNullOrWhiteSpace(empresa.UsuarioSol)
                && !string.IsNullOrWhiteSpace(empresa.ClaveSol)
                && !string.IsNullOrWhiteSpace(empresa.Ubigeo);

            var modo = SunatUrlHelper.NormalizeModo(empresa.SunatModo);

            return new EmpresaFacturacionConfigModel
            {
                EmpresaID = empresa.EmpresaID,
                RazonSocial = empresa.RazonSocial,
                NombreComercial = empresa.NombreComercial,
                RUC = empresa.RUC,
                Direccion = empresa.Direccion,
                Email = empresa.Email,
                Telefono = empresa.Telefono,
                Ubigeo = empresa.Ubigeo,
                GeneraFactElect = empresa.GeneraFactElect == true,
                SunatConfigurado = sunatConfigurado,
                SunatModo = modo,
                SunatWsUrlActivo = SunatUrlHelper.ResolveWsUrl(modo, _configuration),
                RutaCertificadoServidor = SunatUrlHelper.CertificadoFolder,
                SunatIntegrado = sunatIntegrado && sunatConfigurado,
                CertificadoFileName = empresa.CertificadoFileName,
                UsuarioSol = empresa.UsuarioSol
            };
        }

        private static GetAllEmpresaModel Map(Domain.Entities.EmpresaEntity empresa)
        {
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
                BancoNombre = empresa.BancoNombre,
                NumeroCuenta = empresa.NumeroCuenta,
                Cci = empresa.Cci,
                YapeNumero = empresa.YapeNumero,
                PlinNumero = empresa.PlinNumero,
                QrPagoUrl = empresa.QrPagoUrl,
                InstruccionesPago = empresa.InstruccionesPago,
                CuentasPagoJson = empresa.CuentasPagoJson,
                MontoAdelantoReserva = empresa.MontoAdelantoReserva,
                GeneraFactElect = empresa.GeneraFactElect,
                Ubigeo = empresa.Ubigeo,
                RutaCertificadoServidor = empresa.RutaCertificadoServidor,
                CertificadoFileName = empresa.CertificadoFileName,
                TieneCertificado = !string.IsNullOrWhiteSpace(empresa.CertificadoFileName),
                TieneClaveCertificado = !string.IsNullOrWhiteSpace(empresa.ClaveCertificado),
                UsuarioSol = empresa.UsuarioSol,
                TieneClaveSol = !string.IsNullOrWhiteSpace(empresa.ClaveSol),
                SunatModo = empresa.SunatModo,
                SunatWsProduccion = empresa.SunatWsProduccion,
                TieneApiPeruDevToken = !string.IsNullOrWhiteSpace(empresa.ApiPeruDevToken),
                CuentasPago = EmpresaCuentaPagoHelper.Parse(empresa.CuentasPagoJson, empresa),
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
