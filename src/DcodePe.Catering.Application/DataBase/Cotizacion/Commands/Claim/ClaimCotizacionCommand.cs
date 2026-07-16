using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Claim
{
    public class ClaimCotizacionCommand(IDataBaseService databaseService) : IClaimCotizacionCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;

        public async Task<ClaimCotizacionResult> Execute(int cotizacionId, int usuarioId, string usuarioNombre)
        {
            if (cotizacionId <= 0 || usuarioId <= 0)
            {
                return new ClaimCotizacionResult
                {
                    Success = false,
                    Message = "Datos de usuario o cotización inválidos.",
                };
            }

            var entity = await _databaseService.Cotizacion
                .FirstOrDefaultAsync(c => c.CotizacionID == cotizacionId && c.Estado == true);

            if (entity == null)
            {
                return new ClaimCotizacionResult
                {
                    Success = false,
                    Message = "Cotización no encontrada.",
                };
            }

            var origen = (entity.OrigenCotizacion ?? "Admin").Trim();
            if (!origen.Equals("Landing", StringComparison.OrdinalIgnoreCase))
            {
                return new ClaimCotizacionResult
                {
                    Success = true,
                    AlreadyAssigned = true,
                    ResponsableNombre = entity.CreadoPorNombre ?? entity.UsuarioCreacion,
                    FechaAsignacion = entity.FechaAsignacion,
                    Message = "Solo las cotizaciones de Landing pueden tomarse.",
                };
            }

            if (entity.ResponsableUsuarioID.HasValue)
            {
                return new ClaimCotizacionResult
                {
                    Success = true,
                    AlreadyAssigned = true,
                    ResponsableNombre = entity.ResponsableNombre,
                    FechaAsignacion = entity.FechaAsignacion,
                    Message = "La cotización ya fue tomada.",
                };
            }

            var nombre = (usuarioNombre ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                nombre = $"Usuario {usuarioId}";

            entity.ResponsableUsuarioID = usuarioId;
            entity.ResponsableNombre = nombre;
            entity.FechaAsignacion = DateTime.Now;
            entity.UsuarioModificacion = nombre;
            entity.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();

            return new ClaimCotizacionResult
            {
                Success = true,
                AlreadyAssigned = false,
                ResponsableNombre = entity.ResponsableNombre,
                FechaAsignacion = entity.FechaAsignacion,
                Message = "Cotización tomada correctamente.",
            };
        }
    }
}
