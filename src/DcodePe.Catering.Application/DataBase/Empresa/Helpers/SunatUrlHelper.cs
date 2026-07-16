using Microsoft.Extensions.Configuration;

namespace DcodePe.Catering.Application.DataBase.Empresa.Helpers
{
    public static class SunatUrlHelper
    {
        public const string WsProduccionDefault =
            "https://e-factura.sunat.gob.pe/ol-ti-itcpfegem/billService";

        public const string WsDesarrolloDefault =
            "https://e-beta.sunat.gob.pe/ol-ti-itcpfegem-beta/billService";

        public const string CertificadoFolder = "fe/certificado";

        public static string NormalizeModo(string? modo) =>
            string.Equals(modo?.Trim(), "PRODUCCION", StringComparison.OrdinalIgnoreCase)
                ? "PRODUCCION"
                : "DESARROLLO";

        public static string ResolveWsUrl(string? modo, IConfiguration? configuration = null)
        {
            return NormalizeModo(modo) == "PRODUCCION"
                ? configuration?["Sunat:WsProduccion"]?.Trim() ?? WsProduccionDefault
                : configuration?["Sunat:WsDesarrollo"]?.Trim() ?? WsDesarrolloDefault;
        }
    }
}
