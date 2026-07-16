using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using DcodePe.Catering.Application.Database;
using DcodePe.Catering.Application.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DcodePe.Catering.Application.ConsultaDocumento
{
    public class ConsultaDocumentoService(
        HttpClient httpClient,
        IConfiguration configuration,
        IDataBaseService databaseService,
        ISecretProtectionService secretProtection) : IConsultaDocumentoService
    {
        private const string ApiPeruBaseUrl = "https://apiperu.dev";

        private readonly HttpClient _httpClient = httpClient;
        private readonly IDataBaseService _databaseService = databaseService;
        private readonly ISecretProtectionService _secretProtection = secretProtection;
        private readonly string? _configToken = configuration["ConsultaDocumento:ApiPeruToken"];

        public async Task<ConsultaDocumentoResult> ValidateRucAsync(string numero)
        {
            var numeroLimpio = LimpiarNumero(numero);

            if (!EsFormatoRucValido(numeroLimpio))
            {
                return Invalido(numeroLimpio, "RUC", "El RUC debe tener 11 dígitos numéricos.");
            }

            var local = await BuscarClienteLocalAsync(numeroLimpio);
            if (local != null)
            {
                return ExitoLocal(numeroLimpio, "RUC", local);
            }

            var api = await ConsultarRucApiPeruAsync(numeroLimpio);
            if (api != null)
            {
                return api;
            }

            return Invalido(
                numeroLimpio,
                "RUC",
                "No se obtuvo información de SUNAT para ese RUC. Verifique el número o configure el token ApiPeru.dev en Empresa.");
        }

        public async Task<ConsultaDocumentoResult> ValidateDniAsync(string numero)
        {
            var numeroLimpio = LimpiarNumero(numero);

            if (!EsFormatoDniValido(numeroLimpio))
            {
                return Invalido(numeroLimpio, "DNI", "El DNI debe tener 8 dígitos numéricos.");
            }

            var local = await BuscarClienteLocalAsync(numeroLimpio);
            if (local != null)
            {
                return ExitoLocal(numeroLimpio, "DNI", local);
            }

            var api = await ConsultarDniApiPeruAsync(numeroLimpio);
            if (api != null)
            {
                return api;
            }

            return Invalido(
                numeroLimpio,
                "DNI",
                "No se obtuvo información para ese DNI. Verifique el número o configure el token ApiPeru.dev en Empresa.");
        }

        private async Task<LocalClienteInfo?> BuscarClienteLocalAsync(string numeroDocumento)
        {
            var cliente = await _databaseService.Cliente
                .AsNoTracking()
                .Where(c => c.Estado == true && c.NumeroDocumento == numeroDocumento)
                .OrderByDescending(c => c.FechaModificacion ?? c.FechaCreacion)
                .FirstOrDefaultAsync();

            if (cliente == null || string.IsNullOrWhiteSpace(cliente.NombreCompleto))
                return null;

            return new LocalClienteInfo
            {
                Nombre = cliente.NombreCompleto.Trim(),
                Direccion = string.IsNullOrWhiteSpace(cliente.Direccion) ? null : cliente.Direccion.Trim(),
                Telefono = string.IsNullOrWhiteSpace(cliente.Telefono) ? null : cliente.Telefono.Trim()
            };
        }

        private async Task<ConsultaDocumentoResult?> ConsultarRucApiPeruAsync(string numeroLimpio)
        {
            var token = await ObtenerTokenAsync();
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var response = await ConsultarApiPeruGetAsync<ApiPeruRucData>($"/api/ruc/{numeroLimpio}", token)
                ?? await ConsultarApiPeruPostAsync<ApiPeruRucData>("/api/ruc", new { ruc = numeroLimpio }, token);

            if (response?.Data == null)
                return null;

            var razonSocial = (response.Data.NombreORazonSocial ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(razonSocial))
                return null;

            var direccion = (response.Data.DireccionCompleta ?? response.Data.Direccion ?? string.Empty).Trim();

            return new ConsultaDocumentoResult
            {
                EsValido = true,
                NumeroDocumento = LimpiarNumero(response.Data.Ruc ?? numeroLimpio),
                NombreORazonSocial = razonSocial,
                Direccion = string.IsNullOrWhiteSpace(direccion) ? null : direccion,
                Estado = response.Data.Estado,
                Condicion = response.Data.Condicion,
                TipoDocumento = "RUC",
                Fuente = "sunat",
                ConsultaApi = true,
                Mensaje = "Empresa encontrada por SUNAT (ApiPeru.dev)."
            };
        }

        private async Task<ConsultaDocumentoResult?> ConsultarDniApiPeruAsync(string numeroLimpio)
        {
            var token = await ObtenerTokenAsync();
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var response = await ConsultarApiPeruGetAsync<ApiPeruDniData>($"/api/dni/{numeroLimpio}", token)
                ?? await ConsultarApiPeruPostAsync<ApiPeruDniData>("/api/dni", new { dni = numeroLimpio }, token);

            if (response?.Data == null)
                return null;

            var nombreCompleto = (response.Data.NombreCompleto ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(nombreCompleto))
            {
                var nombres = (response.Data.Nombres ?? string.Empty).Trim();
                var apPat = (response.Data.ApellidoPaterno ?? string.Empty).Trim();
                var apMat = (response.Data.ApellidoMaterno ?? string.Empty).Trim();
                nombreCompleto = $"{nombres} {apPat} {apMat}".Trim();
            }

            if (string.IsNullOrWhiteSpace(nombreCompleto))
                return null;

            return new ConsultaDocumentoResult
            {
                EsValido = true,
                NumeroDocumento = numeroLimpio,
                NombreORazonSocial = nombreCompleto,
                TipoDocumento = "DNI",
                Fuente = "reniec",
                ConsultaApi = true,
                Mensaje = "Persona encontrada (ApiPeru.dev)."
            };
        }

        private async Task<ApiPeruResponse<T>?> ConsultarApiPeruGetAsync<T>(string path, string token)
        {
            try
            {
                using var request = CrearRequest(HttpMethod.Get, path, token);
                using var httpResponse = await _httpClient.SendAsync(request);
                if (!httpResponse.IsSuccessStatusCode)
                    return null;

                var payload = await httpResponse.Content.ReadFromJsonAsync<ApiPeruResponse<T>>();
                return payload is { Success: true, Data: not null } ? payload : null;
            }
            catch
            {
                return null;
            }
        }

        private async Task<ApiPeruResponse<T>?> ConsultarApiPeruPostAsync<T>(string path, object body, string token)
        {
            try
            {
                using var request = CrearRequest(HttpMethod.Post, path, token);
                request.Content = JsonContent.Create(body);
                using var httpResponse = await _httpClient.SendAsync(request);
                if (!httpResponse.IsSuccessStatusCode)
                    return null;

                var payload = await httpResponse.Content.ReadFromJsonAsync<ApiPeruResponse<T>>();
                return payload is { Success: true, Data: not null } ? payload : null;
            }
            catch
            {
                return null;
            }
        }

        private HttpRequestMessage CrearRequest(HttpMethod method, string path, string token)
        {
            var request = new HttpRequestMessage(method, $"{ApiPeruBaseUrl}{path}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return request;
        }

        private async Task<string?> ObtenerTokenAsync()
        {
            var empresa = await _databaseService.Empresa
                .AsNoTracking()
                .Where(e => e.Estado == true)
                .OrderBy(e => e.EmpresaID)
                .FirstOrDefaultAsync();

            if (empresa != null && !string.IsNullOrWhiteSpace(empresa.ApiPeruDevToken))
            {
                if (_secretProtection.TryUnprotect(empresa.ApiPeruDevToken, out var plain))
                    return plain.Trim();

                return empresa.ApiPeruDevToken.Trim();
            }

            if (!string.IsNullOrWhiteSpace(_configToken))
                return _configToken.Trim();

            var env = Environment.GetEnvironmentVariable("APIPERU_DEV_TOKEN");
            return string.IsNullOrWhiteSpace(env) ? null : env.Trim();
        }

        private static ConsultaDocumentoResult ExitoLocal(string numero, string tipo, LocalClienteInfo local) =>
            new()
            {
                EsValido = true,
                NumeroDocumento = numero,
                NombreORazonSocial = local.Nombre,
                Direccion = local.Direccion,
                ClienteTelefono = local.Telefono,
                TipoDocumento = tipo,
                Fuente = "local",
                ConsultaApi = true,
                Mensaje = "Cliente encontrado en el sistema."
            };

        private static ConsultaDocumentoResult Invalido(string numero, string tipo, string mensaje) =>
            new()
            {
                EsValido = false,
                NumeroDocumento = numero,
                TipoDocumento = tipo,
                Mensaje = mensaje,
                ConsultaApi = false
            };

        private static string LimpiarNumero(string numero) =>
            Regex.Replace(numero ?? string.Empty, @"\D", string.Empty);

        private static bool EsFormatoRucValido(string numero) =>
            Regex.IsMatch(numero, @"^\d{11}$");

        private static bool EsFormatoDniValido(string numero) =>
            Regex.IsMatch(numero, @"^\d{8}$");

        private sealed class LocalClienteInfo
        {
            public string Nombre { get; set; } = string.Empty;
            public string? Direccion { get; set; }
            public string? Telefono { get; set; }
        }

        private sealed class ApiPeruResponse<T>
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }

            [JsonPropertyName("data")]
            public T? Data { get; set; }
        }

        private sealed class ApiPeruRucData
        {
            [JsonPropertyName("ruc")]
            public string? Ruc { get; set; }

            [JsonPropertyName("nombre_o_razon_social")]
            public string? NombreORazonSocial { get; set; }

            [JsonPropertyName("direccion")]
            public string? Direccion { get; set; }

            [JsonPropertyName("direccion_completa")]
            public string? DireccionCompleta { get; set; }

            [JsonPropertyName("estado")]
            public string? Estado { get; set; }

            [JsonPropertyName("condicion")]
            public string? Condicion { get; set; }
        }

        private sealed class ApiPeruDniData
        {
            [JsonPropertyName("numero")]
            public string? Numero { get; set; }

            [JsonPropertyName("nombre_completo")]
            public string? NombreCompleto { get; set; }

            [JsonPropertyName("nombres")]
            public string? Nombres { get; set; }

            [JsonPropertyName("apellido_paterno")]
            public string? ApellidoPaterno { get; set; }

            [JsonPropertyName("apellido_materno")]
            public string? ApellidoMaterno { get; set; }
        }
    }
}
