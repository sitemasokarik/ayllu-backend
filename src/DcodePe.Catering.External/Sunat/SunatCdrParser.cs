using System.IO.Compression;
using System.Text;
using System.Xml;

namespace DcodePe.Catering.External.Sunat
{
    public class SunatCdrParseResult
    {
        public string ResponseCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CdrXml { get; set; } = string.Empty;
        public bool Aceptado => ResponseCode == "0";
    }

    public class SunatCdrParser
    {
        public SunatCdrParseResult Parse(byte[] cdrZipBytes)
        {
            var cdrXml = ExtractCdrXml(cdrZipBytes);
            var responseCode = ExtractValue(cdrXml, "ResponseCode") ?? string.Empty;
            var description = ExtractValue(cdrXml, "Description") ?? string.Empty;

            return new SunatCdrParseResult
            {
                ResponseCode = responseCode.Trim(),
                Description = description.Trim(),
                CdrXml = cdrXml
            };
        }

        public static string ExtractCdrXml(byte[] cdrZipBytes)
        {
            using var zipStream = new MemoryStream(cdrZipBytes);
            using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);

            var xmlEntry = archive.Entries
                .FirstOrDefault(e => e.FullName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase));

            if (xmlEntry == null)
                throw new InvalidOperationException("El CDR de SUNAT no contiene un archivo XML.");

            using var entryStream = xmlEntry.Open();
            using var reader = new StreamReader(entryStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
            return reader.ReadToEnd();
        }

        private static string? ExtractValue(string xml, string localName)
        {
            var xmlDoc = new XmlDocument { PreserveWhitespace = true };
            xmlDoc.LoadXml(xml);

            var nodes = xmlDoc.GetElementsByTagName(localName);
            return nodes.Count > 0 ? nodes.Item(0)?.InnerText : null;
        }
    }
}
