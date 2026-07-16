using System.Security;
using System.Text;
using System.Xml;

namespace DcodePe.Catering.External.Sunat
{
    public class SunatBillServiceClient
    {
        public async Task<byte[]> SendBillAsync(
            SunatEmpresaConfig config,
            string zipFileName,
            byte[] zipContent,
            CancellationToken cancellationToken = default)
        {
            var username = $"{config.Ruc}{config.UsuarioSol}";
            var contentBase64 = Convert.ToBase64String(zipContent);
            var soapEnvelope = BuildSoapEnvelope(username, config.ClaveSol, zipFileName, contentBase64);

            using var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(2)
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, config.WsUrl);
            request.Headers.Add("SOAPAction", "urn:sendBill");
            request.Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");

            using var response = await httpClient.SendAsync(request, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"SUNAT respondió HTTP {(int)response.StatusCode}: {ExtractFaultMessage(responseBody)}");

            var applicationResponse = ExtractApplicationResponse(responseBody);
            if (applicationResponse == null || applicationResponse.Length == 0)
                throw new InvalidOperationException("SUNAT no devolvió applicationResponse en la respuesta SOAP.");

            return applicationResponse;
        }

        private static string BuildSoapEnvelope(string username, string password, string fileName, string contentBase64)
        {
            return $"""
                <?xml version="1.0" encoding="UTF-8"?>
                <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:ser="http://service.sunat.gob.pe" xmlns:wsse="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
                  <soapenv:Header>
                    <wsse:Security soapenv:mustUnderstand="1">
                      <wsse:UsernameToken>
                        <wsse:Username>{SecurityElement.Escape(username)}</wsse:Username>
                        <wsse:Password Type="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText">{SecurityElement.Escape(password)}</wsse:Password>
                      </wsse:UsernameToken>
                    </wsse:Security>
                  </soapenv:Header>
                  <soapenv:Body>
                    <ser:sendBill>
                      <fileName>{SecurityElement.Escape(fileName)}</fileName>
                      <contentFile>{contentBase64}</contentFile>
                    </ser:sendBill>
                  </soapenv:Body>
                </soapenv:Envelope>
                """;
        }

        private static byte[]? ExtractApplicationResponse(string soapResponse)
        {
            var xmlDoc = new XmlDocument { PreserveWhitespace = true };
            xmlDoc.LoadXml(soapResponse);

            var node = xmlDoc.GetElementsByTagName("applicationResponse").Item(0);
            if (node == null || string.IsNullOrWhiteSpace(node.InnerText))
                return null;

            return Convert.FromBase64String(node.InnerText.Trim());
        }

        private static string ExtractFaultMessage(string soapResponse)
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(soapResponse);

                var faultString = xmlDoc.GetElementsByTagName("faultstring").Item(0)?.InnerText;
                var faultCode = xmlDoc.GetElementsByTagName("faultcode").Item(0)?.InnerText;

                if (!string.IsNullOrWhiteSpace(faultString))
                    return string.IsNullOrWhiteSpace(faultCode) ? faultString : $"{faultCode}: {faultString}";
            }
            catch
            {
                // ignored
            }

            return soapResponse.Length > 500 ? soapResponse[..500] : soapResponse;
        }
    }
}
