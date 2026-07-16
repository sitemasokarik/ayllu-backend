using System.Security.Claims;

namespace DcodePe.Catering.Api.Helpers
{
    public static class PortalAuthHelper
    {
        public static int? GetClienteId(ClaimsPrincipal user)
        {
            if (!string.Equals(user.FindFirst("user_type")?.Value, "cliente", StringComparison.OrdinalIgnoreCase))
                return null;

            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(id, out var clienteId) ? clienteId : null;
        }

        public static bool IsCliente(ClaimsPrincipal user)
        {
            return GetClienteId(user).HasValue;
        }
    }
}
