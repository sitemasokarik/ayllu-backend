using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace DcodePe.Catering.External.Sunat
{
    public class SunatXmlSigner
    {
        private const string SignatureId = "SignatureSP";
        private static readonly object AlgorithmLock = new();
        private static bool _sha1AlgorithmRegistered;

        public string Sign(string xml, string certificadoPath, string claveCertificado)
        {
            EnsureSha1AlgorithmRegistered();

            if (string.IsNullOrWhiteSpace(certificadoPath) || !File.Exists(certificadoPath))
                throw new InvalidOperationException("No se encontró el certificado digital de la empresa.");

            if (string.IsNullOrWhiteSpace(claveCertificado))
                throw new InvalidOperationException("La clave del certificado digital no está configurada.");

            using var certificate = new X509Certificate2(
                certificadoPath,
                claveCertificado,
                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);

            if (!certificate.HasPrivateKey)
                throw new InvalidOperationException("El certificado digital no contiene clave privada.");

            var xmlDoc = new XmlDocument { PreserveWhitespace = false };
            xmlDoc.LoadXml(xml);

            var extensionContent = xmlDoc.GetElementsByTagName("ExtensionContent").Item(0)
                ?? throw new InvalidOperationException("No se encontró ext:ExtensionContent en el XML UBL.");

            var signedXml = new SignedXml(xmlDoc)
            {
                SigningKey = certificate.GetRSAPrivateKey()
                    ?? throw new InvalidOperationException("No se pudo obtener la clave privada RSA del certificado.")
            };

            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;

            var reference = new Reference(string.Empty)
            {
                DigestMethod = SignedXml.XmlDsigSHA1Url
            };
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigC14NTransform());
            signedXml.AddReference(reference);

            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(certificate));
            signedXml.KeyInfo = keyInfo;

            signedXml.ComputeSignature();

            var signatureElement = signedXml.GetXml();
            signatureElement.SetAttribute("Id", SignatureId);

            extensionContent.AppendChild(xmlDoc.ImportNode(signatureElement, true));

            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false),
                OmitXmlDeclaration = false,
                Indent = false
            });
            xmlDoc.Save(xmlWriter);
            return stringWriter.ToString();
        }

        private static void EnsureSha1AlgorithmRegistered()
        {
            if (_sha1AlgorithmRegistered)
                return;

            lock (AlgorithmLock)
            {
                if (_sha1AlgorithmRegistered)
                    return;

                try
                {
                    CryptoConfig.AddAlgorithm(typeof(RsaPkcs1Sha1SignatureDescription), SignedXml.XmlDsigRSASHA1Url);
                }
                catch (CryptographicException)
                {
                    // Algoritmo ya registrado en el AppDomain.
                }

                _sha1AlgorithmRegistered = true;
            }
        }

        private sealed class RsaPkcs1Sha1SignatureDescription : SignatureDescription
        {
            public RsaPkcs1Sha1SignatureDescription()
            {
                KeyAlgorithm = typeof(RSA).FullName!;
                DigestAlgorithm = typeof(SHA1).FullName!;
                FormatterAlgorithm = typeof(RSAPKCS1SignatureFormatter).FullName!;
                DeformatterAlgorithm = typeof(RSAPKCS1SignatureDeformatter).FullName!;
            }

            public override AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key) =>
                new RSAPKCS1SignatureFormatter(key);

            public override AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key) =>
                new RSAPKCS1SignatureDeformatter(key);

            public override HashAlgorithm CreateDigest() => SHA1.Create();
        }
    }
}
