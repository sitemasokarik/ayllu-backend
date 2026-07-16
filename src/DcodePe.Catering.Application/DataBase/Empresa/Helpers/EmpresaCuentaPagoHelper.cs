using System.Text.Json;
using System.Text.Json.Serialization;

namespace DcodePe.Catering.Application.DataBase.Empresa.Helpers
{
    public class EmpresaCuentaPagoItem
    {
        public string? Alias { get; set; }
        public string? BancoNombre { get; set; }
        public string? NumeroCuenta { get; set; }
        public string? Cci { get; set; }
        public string? YapeNumero { get; set; }
        public string? PlinNumero { get; set; }
    }

    public static class EmpresaCuentaPagoHelper
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static List<EmpresaCuentaPagoItem> Parse(string? json, Domain.Entities.EmpresaEntity? empresa = null)
        {
            if (!string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    var parsed = JsonSerializer.Deserialize<List<EmpresaCuentaPagoItem>>(json, JsonOptions);
                    if (parsed?.Count > 0)
                        return parsed.Where(c => HasData(c)).ToList();
                }
                catch
                {
                    // fallback below
                }
            }

            if (empresa == null)
                return [];

            var legacy = new EmpresaCuentaPagoItem
            {
                Alias = "Cuenta principal",
                BancoNombre = empresa.BancoNombre,
                NumeroCuenta = empresa.NumeroCuenta,
                Cci = empresa.Cci,
                YapeNumero = empresa.YapeNumero,
                PlinNumero = empresa.PlinNumero
            };

            return HasData(legacy) ? [legacy] : [];
        }

        public static string Serialize(IEnumerable<EmpresaCuentaPagoItem>? cuentas)
        {
            var list = (cuentas ?? []).Where(HasData).ToList();
            return list.Count == 0 ? string.Empty : JsonSerializer.Serialize(list, JsonOptions);
        }

        private static bool HasData(EmpresaCuentaPagoItem? c)
        {
            if (c == null) return false;
            return !string.IsNullOrWhiteSpace(c.BancoNombre)
                || !string.IsNullOrWhiteSpace(c.NumeroCuenta)
                || !string.IsNullOrWhiteSpace(c.Cci)
                || !string.IsNullOrWhiteSpace(c.YapeNumero)
                || !string.IsNullOrWhiteSpace(c.PlinNumero);
        }
    }
}
