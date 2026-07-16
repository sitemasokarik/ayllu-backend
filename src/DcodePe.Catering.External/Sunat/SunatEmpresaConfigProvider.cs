using DcodePe.Catering.Application.Database;
using DcodePe.Catering.Application.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DcodePe.Catering.External.Sunat
{
    public class SunatEmpresaConfigProvider(
        IDataBaseService databaseService,
        ISecretProtectionService secretProtection,
        IConfiguration configuration)
    {
        public const string DefaultWsProduccion =
            "https://e-factura.sunat.gob.pe/ol-ti-itcpfegem/billService";

        public const string DefaultWsDesarrollo =
            "https://e-beta.sunat.gob.pe/ol-ti-itcpfegem-beta/billService";

        public const string CertificadoFolder = "fe/certificado";

        private readonly IDataBaseService _databaseService = databaseService;
        private readonly ISecretProtectionService _secretProtection = secretProtection;
        private readonly IConfiguration _configuration = configuration;

        public async Task<SunatEmpresaConfig?> GetConfigAsync(CancellationToken cancellationToken = default)
        {
            var empresa = await _databaseService.Empresa
                .Where(e => e.Estado == true)
                .OrderBy(e => e.EmpresaID)
                .FirstOrDefaultAsync(cancellationToken);

            if (empresa == null)
                return null;

            var claveCertificado = UnprotectSecret(empresa.ClaveCertificado);
            var claveSol = UnprotectSecret(empresa.ClaveSol);

            var certificadoFileName = empresa.CertificadoFileName ?? string.Empty;
            var certificadoPath = string.IsNullOrWhiteSpace(certificadoFileName)
                ? string.Empty
                : Path.Combine(AppContext.BaseDirectory, CertificadoFolder, certificadoFileName);

            var modo = (empresa.SunatModo ?? "DESARROLLO").Trim().ToUpperInvariant();
            var wsUrl = modo == "PRODUCCION"
                ? (_configuration["Sunat:WsProduccion"]?.Trim() ?? DefaultWsProduccion)
                : (_configuration["Sunat:WsDesarrollo"]?.Trim() ?? DefaultWsDesarrollo);

            return new SunatEmpresaConfig
            {
                Ruc = empresa.RUC?.Trim() ?? string.Empty,
                RazonSocial = empresa.RazonSocial?.Trim() ?? string.Empty,
                Ubigeo = empresa.Ubigeo?.Trim() ?? string.Empty,
                Direccion = empresa.Direccion?.Trim() ?? string.Empty,
                UsuarioSol = empresa.UsuarioSol?.Trim() ?? string.Empty,
                ClaveSol = claveSol,
                CertificadoPath = certificadoPath,
                ClaveCertificado = claveCertificado,
                WsUrl = wsUrl,
                SunatModo = modo,
                GeneraFactElect = empresa.GeneraFactElect == true
            };
        }

        private string UnprotectSecret(string? protectedValue)
        {
            if (string.IsNullOrWhiteSpace(protectedValue))
                return string.Empty;

            return _secretProtection.TryUnprotect(protectedValue, out var plainText)
                ? plainText
                : protectedValue;
        }
    }
}
