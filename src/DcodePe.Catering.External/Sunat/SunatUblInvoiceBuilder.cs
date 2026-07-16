using System.Globalization;
using System.Security;
using System.Text;
using System.Xml;
using DcodePe.Catering.Domain.Entities.Facturacion;

namespace DcodePe.Catering.External.Sunat
{
    public class SunatUblInvoiceBuilder
    {
        private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

        public string Build(ComprobanteElectronicoEntity comprobante, SunatEmpresaConfig config)
        {
            var tipoComprobante = MapTipoComprobante(comprobante.Tipo);
            var tipoDocCliente = MapTipoDocumentoCliente(comprobante.TipoDocumento);
            var issueDate = comprobante.FechaEmision.ToString("yyyy-MM-dd", Invariant);
            var issueTime = comprobante.FechaEmision.ToString("HH:mm:ss", Invariant);
            var documentId = $"{comprobante.Serie}-{comprobante.Correlativo}";
            var ubigeo = string.IsNullOrWhiteSpace(config.Ubigeo) ? "-" : config.Ubigeo;
            var direccionEmisor = EscapeCdata(config.Direccion);
            var direccionCliente = EscapeCdata(comprobante.ClienteDireccion ?? "-");
            var razonSocialEmisor = EscapeCdata(config.RazonSocial);
            var razonSocialCliente = EscapeCdata(comprobante.ClienteNombre);
            var detalles = comprobante.Detalles
                .Where(d => d.Estado != false)
                .OrderBy(d => d.Item)
                .ToList();

            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.Append("<Invoice xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:Invoice-2\"");
            sb.Append(" xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\"");
            sb.Append(" xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\"");
            sb.Append(" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\"");
            sb.Append(" xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\">");

            sb.Append("<ext:UBLExtensions>");
            sb.Append("<ext:UBLExtension>");
            sb.Append("<ext:ExtensionContent/>");
            sb.Append("</ext:UBLExtension>");
            sb.Append("</ext:UBLExtensions>");

            sb.Append("<cbc:UBLVersionID>2.1</cbc:UBLVersionID>");
            sb.Append("<cbc:CustomizationID>2.0</cbc:CustomizationID>");
            sb.Append($"<cbc:ID>{documentId}</cbc:ID>");
            sb.Append($"<cbc:IssueDate>{issueDate}</cbc:IssueDate>");
            sb.Append($"<cbc:IssueTime>{issueTime}</cbc:IssueTime>");
            sb.Append("<cbc:InvoiceTypeCode listAgencyName=\"PE:SUNAT\" listName=\"Tipo de Documento\" listURI=\"urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo01\">");
            sb.Append(tipoComprobante);
            sb.Append("</cbc:InvoiceTypeCode>");
            sb.Append($"<cbc:DocumentCurrencyCode listID=\"ISO 4217 Alpha\" listName=\"Currency\" listAgencyName=\"United Nations Economic Commission for Europe\">{comprobante.Moneda}</cbc:DocumentCurrencyCode>");

            sb.Append("<cac:Signature>");
            sb.Append("<cbc:ID>IDSignKG</cbc:ID>");
            sb.Append("<cac:SignatoryParty>");
            sb.Append("<cac:PartyIdentification>");
            sb.Append($"<cbc:ID>{config.Ruc}</cbc:ID>");
            sb.Append("</cac:PartyIdentification>");
            sb.Append("<cac:PartyName>");
            sb.Append($"<cbc:Name><![CDATA[{razonSocialEmisor}]]></cbc:Name>");
            sb.Append("</cac:PartyName>");
            sb.Append("</cac:SignatoryParty>");
            sb.Append("<cac:DigitalSignatureAttachment>");
            sb.Append("<cac:ExternalReference>");
            sb.Append("<cbc:URI>#SignatureSP</cbc:URI>");
            sb.Append("</cac:ExternalReference>");
            sb.Append("</cac:DigitalSignatureAttachment>");
            sb.Append("</cac:Signature>");

            AppendSupplierParty(sb, config, ubigeo, direccionEmisor, razonSocialEmisor);
            AppendCustomerParty(sb, comprobante, tipoDocCliente, ubigeo, direccionCliente, razonSocialCliente);

            sb.Append("<cac:TaxTotal>");
            sb.Append($"<cbc:TaxAmount currencyID=\"{comprobante.Moneda}\">{FormatAmount(comprobante.Igv)}</cbc:TaxAmount>");
            sb.Append("<cac:TaxSubtotal>");
            sb.Append($"<cbc:TaxableAmount currencyID=\"{comprobante.Moneda}\">{FormatAmount(comprobante.OpGravadas)}</cbc:TaxableAmount>");
            sb.Append($"<cbc:TaxAmount currencyID=\"{comprobante.Moneda}\">{FormatAmount(comprobante.Igv)}</cbc:TaxAmount>");
            sb.Append("<cac:TaxCategory>");
            sb.Append("<cac:TaxScheme>");
            sb.Append("<cbc:ID schemeID=\"UN/ECE 5305\" schemeName=\"Tax Category Identifier\" schemeAgencyName=\"United Nations Economic Commission for Europe\">S</cbc:ID>");
            sb.Append("<cbc:Name>IGV</cbc:Name>");
            sb.Append("<cbc:TaxTypeCode>VAT</cbc:TaxTypeCode>");
            sb.Append("</cac:TaxScheme>");
            sb.Append("</cac:TaxCategory>");
            sb.Append("</cac:TaxSubtotal>");
            sb.Append("</cac:TaxTotal>");

            sb.Append("<cac:LegalMonetaryTotal>");
            sb.Append($"<cbc:LineExtensionAmount currencyID=\"{comprobante.Moneda}\">{FormatAmount(comprobante.Subtotal)}</cbc:LineExtensionAmount>");
            sb.Append($"<cbc:TaxInclusiveAmount currencyID=\"{comprobante.Moneda}\">{FormatAmount(comprobante.Total)}</cbc:TaxInclusiveAmount>");
            sb.Append($"<cbc:PayableAmount currencyID=\"{comprobante.Moneda}\">{FormatAmount(comprobante.Total)}</cbc:PayableAmount>");
            sb.Append("</cac:LegalMonetaryTotal>");

            foreach (var detalle in detalles)
                AppendInvoiceLine(sb, detalle, comprobante.Moneda);

            sb.Append("<cac:PaymentTerms>");
            sb.Append("<cbc:ID>FormaPago</cbc:ID>");
            sb.Append("<cbc:PaymentMeansID>Contado</cbc:PaymentMeansID>");
            sb.Append("</cac:PaymentTerms>");

            sb.Append("</Invoice>");

            var xml = sb.ToString();
            ValidateXml(xml);
            return xml;
        }

        public static string BuildFileBaseName(SunatEmpresaConfig config, ComprobanteElectronicoEntity comprobante)
        {
            var tipo = MapTipoComprobante(comprobante.Tipo);
            var correlativo = ParseCorrelativo(comprobante.Correlativo);
            return $"{config.Ruc}-{tipo}-{comprobante.Serie}-{correlativo}";
        }

        private static void AppendSupplierParty(
            StringBuilder sb,
            SunatEmpresaConfig config,
            string ubigeo,
            string direccion,
            string razonSocial)
        {
            sb.Append("<cac:AccountingSupplierParty>");
            sb.Append("<cac:Party>");
            sb.Append("<cac:PartyIdentification>");
            sb.Append("<cbc:ID schemeID=\"6\" schemeName=\"Documento de Identidad\" schemeAgencyName=\"PE:SUNAT\" schemeURI=\"urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06\">");
            sb.Append(config.Ruc);
            sb.Append("</cbc:ID>");
            sb.Append("</cac:PartyIdentification>");
            sb.Append("<cac:PartyName>");
            sb.Append($"<cbc:Name><![CDATA[{razonSocial}]]></cbc:Name>");
            sb.Append("</cac:PartyName>");
            sb.Append("<cac:PartyTaxScheme>");
            sb.Append("<cbc:RegistrationName><![CDATA[").Append(razonSocial).Append("]]></cbc:RegistrationName>");
            sb.Append("<cbc:CompanyID schemeID=\"6\" schemeName=\"Documento de Identidad\" schemeAgencyName=\"PE:SUNAT\" schemeURI=\"urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06\">");
            sb.Append(config.Ruc);
            sb.Append("</cbc:CompanyID>");
            sb.Append("<cac:TaxScheme>");
            sb.Append("<cbc:ID schemeID=\"UN/ECE 5153\" schemeAgencyID=\"6\">VAT</cbc:ID>");
            sb.Append("</cac:TaxScheme>");
            sb.Append("</cac:PartyTaxScheme>");
            sb.Append("<cac:PartyLegalEntity>");
            sb.Append("<cbc:RegistrationName><![CDATA[").Append(razonSocial).Append("]]></cbc:RegistrationName>");
            sb.Append("<cac:RegistrationAddress>");
            sb.Append($"<cbc:ID schemeName=\"Ubigeos\" schemeAgencyName=\"PE:INEI\">{ubigeo}</cbc:ID>");
            sb.Append("<cbc:AddressTypeCode listAgencyName=\"PE:SUNAT\" listName=\"Establecimientos anexos\">0000</cbc:AddressTypeCode>");
            sb.Append($"<cbc:CityName>-</cbc:CityName>");
            sb.Append($"<cbc:CountrySubentity>-</cbc:CountrySubentity>");
            sb.Append($"<cbc:District>-</cbc:District>");
            sb.Append($"<cbc:AddressLine><![CDATA[{direccion}]]></cbc:AddressLine>");
            sb.Append("<cbc:Country>");
            sb.Append("<cbc:IdentificationCode listID=\"ISO 3166-1\" listAgencyName=\"United Nations Economic Commission for Europe\" listName=\"Country\">PE</cbc:IdentificationCode>");
            sb.Append("</cbc:Country>");
            sb.Append("</cac:RegistrationAddress>");
            sb.Append("</cac:PartyLegalEntity>");
            sb.Append("</cac:Party>");
            sb.Append("</cac:AccountingSupplierParty>");
        }

        private static void AppendCustomerParty(
            StringBuilder sb,
            ComprobanteElectronicoEntity comprobante,
            string tipoDocCliente,
            string ubigeo,
            string direccion,
            string razonSocial)
        {
            sb.Append("<cac:AccountingCustomerParty>");
            sb.Append("<cac:Party>");
            sb.Append("<cac:PartyIdentification>");
            sb.Append($"<cbc:ID schemeID=\"{tipoDocCliente}\" schemeName=\"Documento de Identidad\" schemeAgencyName=\"PE:SUNAT\" schemeURI=\"urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06\">");
            sb.Append(EscapeText(comprobante.ClienteDocumento));
            sb.Append("</cbc:ID>");
            sb.Append("</cac:PartyIdentification>");
            sb.Append("<cac:PartyLegalEntity>");
            sb.Append($"<cbc:RegistrationName><![CDATA[{razonSocial}]]></cbc:RegistrationName>");
            sb.Append("<cac:RegistrationAddress>");
            sb.Append($"<cbc:ID schemeName=\"Ubigeos\" schemeAgencyName=\"PE:INEI\">{ubigeo}</cbc:ID>");
            sb.Append($"<cbc:CityName>-</cbc:CityName>");
            sb.Append($"<cbc:CountrySubentity>-</cbc:CountrySubentity>");
            sb.Append($"<cbc:District>-</cbc:District>");
            sb.Append($"<cbc:AddressLine><![CDATA[{direccion}]]></cbc:AddressLine>");
            sb.Append("<cbc:Country>");
            sb.Append("<cbc:IdentificationCode listID=\"ISO 3166-1\" listAgencyName=\"United Nations Economic Commission for Europe\" listName=\"Country\">PE</cbc:IdentificationCode>");
            sb.Append("</cbc:Country>");
            sb.Append("</cac:RegistrationAddress>");
            sb.Append("</cac:PartyLegalEntity>");
            sb.Append("</cac:Party>");
            sb.Append("</cac:AccountingCustomerParty>");
        }

        private static void AppendInvoiceLine(StringBuilder sb, ComprobanteDetalleEntity detalle, string moneda)
        {
            sb.Append("<cac:InvoiceLine>");
            sb.Append($"<cbc:ID>{detalle.Item}</cbc:ID>");
            sb.Append($"<cbc:InvoicedQuantity unitCode=\"{EscapeText(detalle.UnidadMedida)}\" unitCodeListID=\"UN/ECE rec 20\" unitCodeListAgencyName=\"United Nations Economic Commission for Europe\">{FormatQuantity(detalle.Cantidad)}</cbc:InvoicedQuantity>");
            sb.Append($"<cbc:LineExtensionAmount currencyID=\"{moneda}\">{FormatAmount(detalle.Subtotal)}</cbc:LineExtensionAmount>");

            sb.Append("<cac:PricingReference>");
            sb.Append("<cac:AlternativeConditionPrice>");
            sb.Append($"<cbc:PriceAmount currencyID=\"{moneda}\">{FormatAmount(detalle.Importe)}</cbc:PriceAmount>");
            sb.Append("<cbc:PriceTypeCode listAgencyName=\"PE:SUNAT\" listName=\"Tipo de Precio\" listURI=\"urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo16\">01</cbc:PriceTypeCode>");
            sb.Append("</cac:AlternativeConditionPrice>");
            sb.Append("</cac:PricingReference>");

            sb.Append("<cac:TaxTotal>");
            sb.Append($"<cbc:TaxAmount currencyID=\"{moneda}\">{FormatAmount(detalle.Igv)}</cbc:TaxAmount>");
            sb.Append("<cac:TaxSubtotal>");
            sb.Append($"<cbc:TaxableAmount currencyID=\"{moneda}\">{FormatAmount(detalle.Subtotal)}</cbc:TaxableAmount>");
            sb.Append($"<cbc:TaxAmount currencyID=\"{moneda}\">{FormatAmount(detalle.Igv)}</cbc:TaxAmount>");
            sb.Append("<cac:TaxCategory>");
            sb.Append($"<cbc:Percent>{FormatAmount(18m)}</cbc:Percent>");
            sb.Append($"<cbc:TaxExemptionReasonCode listAgencyName=\"PE:SUNAT\" listName=\"Afectacion del IGV\" listURI=\"urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo07\">{EscapeText(detalle.IdTipoIgv)}</cbc:TaxExemptionReasonCode>");
            sb.Append("<cac:TaxScheme>");
            sb.Append("<cbc:ID schemeID=\"UN/ECE 5305\" schemeName=\"Tax Category Identifier\" schemeAgencyName=\"United Nations Economic Commission for Europe\">S</cbc:ID>");
            sb.Append("<cbc:Name>IGV</cbc:Name>");
            sb.Append("<cbc:TaxTypeCode>VAT</cbc:TaxTypeCode>");
            sb.Append("</cac:TaxScheme>");
            sb.Append("</cac:TaxCategory>");
            sb.Append("</cac:TaxSubtotal>");
            sb.Append("</cac:TaxTotal>");

            sb.Append("<cac:Item>");
            sb.Append($"<cbc:Description><![CDATA[{EscapeCdata(detalle.Descripcion)}]]></cbc:Description>");
            sb.Append("<cac:SellersItemIdentification>");
            sb.Append($"<cbc:ID>{EscapeText(detalle.Codigo)}</cbc:ID>");
            sb.Append("</cac:SellersItemIdentification>");
            sb.Append("</cac:Item>");

            sb.Append("<cac:Price>");
            sb.Append($"<cbc:PriceAmount currencyID=\"{moneda}\">{FormatAmount(detalle.Valor)}</cbc:PriceAmount>");
            sb.Append("</cac:Price>");
            sb.Append("</cac:InvoiceLine>");
        }

        private static string MapTipoComprobante(string tipo) =>
            tipo.Equals("factura", StringComparison.OrdinalIgnoreCase) ? "01" : "03";

        private static string MapTipoDocumentoCliente(string? tipoDocumento)
        {
            if (string.IsNullOrWhiteSpace(tipoDocumento))
                return "1";

            return tipoDocumento.Trim().ToUpperInvariant() switch
            {
                "DNI" => "1",
                "RUC" => "6",
                "CE" => "4",
                "PASAPORTE" or "PAS" => "7",
                _ when int.TryParse(tipoDocumento, out _) => tipoDocumento.Trim(),
                _ => "1"
            };
        }

        private static int ParseCorrelativo(string correlativo)
        {
            if (int.TryParse(correlativo, out var value))
                return value;

            return 1;
        }

        private static string FormatAmount(decimal value) =>
            value.ToString("0.00", Invariant);

        private static string FormatQuantity(decimal value) =>
            value.ToString("0.000", Invariant);

        private static string EscapeText(string? value) =>
            SecurityElement.Escape(value ?? string.Empty) ?? string.Empty;

        private static string EscapeCdata(string? value) =>
            (value ?? string.Empty).Replace("]]>", "]]]]><![CDATA[>");

        private static void ValidateXml(string xml)
        {
            var settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null
            };

            using var reader = XmlReader.Create(new StringReader(xml), settings);
            while (reader.Read())
            {
            }
        }
    }
}
