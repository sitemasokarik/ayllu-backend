using DcodePe.Catering.Application.Security;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Cliente.Commands.RegisterPortal
{
    public class RegisterClientePortalCommand(
        IDataBaseService databaseService,
        IPasswordHashService passwordHashService) : IRegisterClientePortalCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;
        private readonly IPasswordHashService _passwordHashService = passwordHashService;

        public async Task<RegisterClientePortalModel> Execute(RegisterClientePortalModel model)
        {
            var emailNormalizado = model.Email.Trim().ToLowerInvariant();
            var dni = model.NumeroDocumento.Replace(" ", string.Empty).Trim();
            var telefono = model.Telefono?.Replace(" ", string.Empty).Trim() ?? string.Empty;
            var nombreCompleto = $"{model.Nombres.Trim()} {model.Apellidos.Trim()}".Trim();

            if (dni.Length != 8 || !dni.All(char.IsDigit))
                throw new InvalidOperationException("El DNI debe tener 8 dígitos.");

            if (string.IsNullOrWhiteSpace(nombreCompleto))
                throw new InvalidOperationException("Ingresa nombres y apellidos.");

            if (string.IsNullOrWhiteSpace(telefono))
                throw new InvalidOperationException("El teléfono es obligatorio.");

            var existingByDni = await _databaseService.Cliente
                .Include(c => c.Cotizaciones)
                .FirstOrDefaultAsync(c => c.Estado == true && c.NumeroDocumento == dni);

            if (existingByDni?.EsPortalActivo == true)
                throw new InvalidOperationException("Ya existe una cuenta de portal con este DNI.");

            await EnsureUniqueContactAsync(emailNormalizado, telefono, existingByDni?.ClienteID);

            if (existingByDni != null)
            {
                existingByDni.NombreCompleto = nombreCompleto;
                existingByDni.Email = emailNormalizado;
                existingByDni.Telefono = telefono;
                existingByDni.TipoDocumento = "DNI";
                existingByDni.UserNamePortal = emailNormalizado;
                existingByDni.PasswordHash = _passwordHashService.HashPassword(model.Password);
                existingByDni.EsPortalActivo = true;
                existingByDni.UsuarioModificacion = emailNormalizado;
                existingByDni.FechaModificacion = DateTime.Now;

                await _databaseService.SaveAsync();

                model.ClienteID = existingByDni.ClienteID;
                model.TotalCotizacionesVinculadas = existingByDni.Cotizaciones.Count(c => c.Estado == true);
                model.Password = string.Empty;
                return model;
            }

            var entity = new Domain.Entities.Clientes.ClienteEntity
            {
                TipoDocumento = "DNI",
                NumeroDocumento = dni,
                NombreCompleto = nombreCompleto,
                Email = emailNormalizado,
                Telefono = telefono,
                TipoCliente = "Natural",
                UserNamePortal = emailNormalizado,
                PasswordHash = _passwordHashService.HashPassword(model.Password),
                EsPortalActivo = true,
                EsVIP = false,
                UsuarioCreacion = emailNormalizado,
                FechaCreacion = DateTime.Now,
                Estado = true
            };

            await _databaseService.Cliente.AddAsync(entity);
            await _databaseService.SaveAsync();

            model.ClienteID = entity.ClienteID;
            model.TotalCotizacionesVinculadas = 0;
            model.Password = string.Empty;
            return model;
        }

        private async Task EnsureUniqueContactAsync(string email, string telefono, int? excludeClienteId)
        {
            await LiberarContactoDuplicadoSinUsoAsync(email, telefono, excludeClienteId);

            var query = _databaseService.Cliente.Where(c => c.Estado == true);
            if (excludeClienteId.HasValue)
                query = query.Where(c => c.ClienteID != excludeClienteId.Value);

            if (await query.AnyAsync(c => c.Email == email || c.UserNamePortal == email))
                throw new InvalidOperationException("Ya existe un cliente registrado con ese correo.");

            if (await query.AnyAsync(c => c.Telefono == telefono || c.TelefonoSecundario == telefono))
                throw new InvalidOperationException("Ya existe un cliente registrado con ese teléfono.");
        }

        private async Task LiberarContactoDuplicadoSinUsoAsync(string email, string telefono, int? excludeClienteId)
        {
            var candidatos = await _databaseService.Cliente
                .Include(c => c.Cotizaciones)
                .Where(c =>
                    c.Estado == true &&
                    (!excludeClienteId.HasValue || c.ClienteID != excludeClienteId.Value) &&
                    (c.Email == email || c.UserNamePortal == email || c.Telefono == telefono || c.TelefonoSecundario == telefono))
                .ToListAsync();

            var changed = false;
            foreach (var c in candidatos)
            {
                if (c.EsPortalActivo || c.Cotizaciones.Any(x => x.Estado == true))
                    continue;

                if (c.Email == email || c.UserNamePortal == email)
                {
                    c.Email = $"archivado-{c.ClienteID}@ayllu.local";
                    c.UserNamePortal = null;
                    changed = true;
                }

                if (c.Telefono == telefono)
                {
                    c.Telefono = $"0000000{c.ClienteID}";
                    changed = true;
                }
            }

            if (changed)
                await _databaseService.SaveAsync();
        }
    }
}
