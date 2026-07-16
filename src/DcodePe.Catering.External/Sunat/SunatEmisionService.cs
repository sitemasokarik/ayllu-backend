using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using DcodePe.Catering.Application.External.Sunat;
using DcodePe.Catering.Domain.Entities.Facturacion;
using Microsoft.Extensions.Configuration;

namespace DcodePe.Catering.External.Sunat
{
    public class SunatEmisionService(
        IConfiguration configuration,
        SunatEmpresaConfigProvider configProvider,
        SunatUblInvoiceBuilder ublInvoiceBuilder,
        SunatXmlSigner xmlSigner,
        SunatBillServiceClient billServiceClient,
        SunatCdrParser cdrParser) : ISunatEmisionService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly SunatEmpresaConfigProvider _configProvider = configProvider;
        private readonly SunatUblInvoiceBuilder _ublInvoiceBuilder = ublInvoiceBuilder;
        private readonly SunatXmlSigner _xmlSigner = xmlSigner;
        private readonly SunatBillServiceClient _billServiceClient = billServiceClient;
        private readonly SunatCdrParser _cdrParser = cdrParser;

        public async Task<SunatEmisionResult> EmitirComprobanteAsync(ComprobanteElectronicoEntity comprobante)
        {
            var integrado = bool.TryParse(_configuration["Sunat:Integrado"], out var integradoValue) && integradoValue;

            if (!integrado)
            {
                return Pendiente(
                    "Integración SUNAT desactivada. El comprobante quedará pendiente hasta activarla en configuración.");
            }

            var config = await _configProvider.GetConfigAsync();
            if (config == null)
            {
                return Pendiente("No hay empresa activa configurada para facturación electrónica.");
            }

            if (!IsSunatConfigComplete(config))
            {
                return Pendiente(
                    "Complete la facturación electrónica en Empresa (certificado, SOL, ubigeo) para enviar a SUNAT.");
            }

            try
            {
                ValidateConfig(config);

                var unsignedXml = _ublInvoiceBuilder.Build(comprobante, config);
                var signedXml = _xmlSigner.Sign(unsignedXml, config.CertificadoPath, config.ClaveCertificado);
                var fileBaseName = SunatUblInvoiceBuilder.BuildFileBaseName(config, comprobante);
                var xmlFileName = $"{fileBaseName}.xml";
                var zipFileName = $"{fileBaseName}.zip";
                var zipBytes = CreateZip(xmlFileName, signedXml);

                var cdrZipBytes = await _billServiceClient.SendBillAsync(config, zipFileName, zipBytes);
                var cdrResult = _cdrParser.Parse(cdrZipBytes);

                var xmlDir = Path.Combine(AppContext.BaseDirectory, "fe", "xml");
                var cdrDir = Path.Combine(AppContext.BaseDirectory, "fe", "cdr");
                Directory.CreateDirectory(xmlDir);
                Directory.CreateDirectory(cdrDir);

                var xmlPath = Path.Combine(xmlDir, xmlFileName);
                var cdrZipPath = Path.Combine(cdrDir, $"{fileBaseName}.zip");
                var cdrXmlPath = Path.Combine(cdrDir, $"R-{fileBaseName}.xml");

                await File.WriteAllTextAsync(xmlPath, signedXml, new UTF8Encoding(false));
                await File.WriteAllBytesAsync(cdrZipPath, cdrZipBytes);
                await File.WriteAllTextAsync(cdrXmlPath, cdrResult.CdrXml, new UTF8Encoding(false));

                var hash = ComputeSha256Hash(signedXml);
                var aceptado = cdrResult.Aceptado;

                return new SunatEmisionResult
                {
                    Integrado = true,
                    Ticket = fileBaseName,
                    EstadoComprobante = aceptado ? "Aceptado" : "Rechazado",
                    Respuesta = cdrResult.Description,
                    SunatHashCpe = hash,
                    RutaXml = Path.Combine("fe", "xml", xmlFileName),
                    RutaCdr = Path.Combine("fe", "cdr", $"{fileBaseName}.zip"),
                    SunatCodigoError = aceptado ? null : cdrResult.ResponseCode,
                    SunatCdr = cdrResult.CdrXml
                };
            }
            catch (Exception ex)
            {
                return Rechazado(ex.Message, "ERROR");
            }
        }

        private static void ValidateConfig(SunatEmpresaConfig config)
        {
            if (string.IsNullOrWhiteSpace(config.Ruc))
                throw new InvalidOperationException("El RUC de la empresa no está configurado.");

            if (string.IsNullOrWhiteSpace(config.UsuarioSol) || string.IsNullOrWhiteSpace(config.ClaveSol))
                throw new InvalidOperationException("Las credenciales SOL de la empresa no están configuradas.");

            if (string.IsNullOrWhiteSpace(config.CertificadoPath) || !File.Exists(config.CertificadoPath))
                throw new InvalidOperationException("No se encontró el certificado digital (.pfx) de la empresa.");

            if (string.IsNullOrWhiteSpace(config.ClaveCertificado))
                throw new InvalidOperationException("La clave del certificado digital no está configurada.");

            if (string.IsNullOrWhiteSpace(config.Ubigeo))
                throw new InvalidOperationException("El ubigeo de la empresa no está configurado.");
        }

        private static byte[] CreateZip(string entryName, string xmlContent)
        {
            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
                using var entryStream = entry.Open();
                var bytes = new UTF8Encoding(false).GetBytes(xmlContent);
                entryStream.Write(bytes, 0, bytes.Length);
            }

            return memoryStream.ToArray();
        }

        private static string ComputeSha256Hash(string content)
        {
            var bytes = new UTF8Encoding(false).GetBytes(content);
            var hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        private static bool IsSunatConfigComplete(SunatEmpresaConfig config) =>
            config.GeneraFactElect
            && !string.IsNullOrWhiteSpace(config.Ruc)
            && !string.IsNullOrWhiteSpace(config.UsuarioSol)
            && !string.IsNullOrWhiteSpace(config.ClaveSol)
            && !string.IsNullOrWhiteSpace(config.ClaveCertificado)
            && !string.IsNullOrWhiteSpace(config.CertificadoPath)
            && File.Exists(config.CertificadoPath)
            && !string.IsNullOrWhiteSpace(config.Ubigeo);

        private static SunatEmisionResult Pendiente(string message) =>
            new()
            {
                Integrado = false,
                EstadoComprobante = "Pendiente SUNAT",
                Respuesta = message
            };

        private static SunatEmisionResult Rechazado(string message, string codigo) =>
            new()
            {
                Integrado = true,
                EstadoComprobante = "Rechazado",
                Respuesta = message,
                SunatCodigoError = codigo
            };
    }
}
