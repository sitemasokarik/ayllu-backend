using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Cliente.Commands.UpdatePortalProfile
{
    public class UpdateClientePortalProfileCommand(IDataBaseService databaseService) : IUpdateClientePortalProfileCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;

        public async Task<UpdateClientePortalProfileModel> Execute(int clienteId, UpdateClientePortalProfileModel model)
        {
            var cliente = await _databaseService.Cliente
                .FirstOrDefaultAsync(c => c.ClienteID == clienteId && c.Estado == true && c.EsPortalActivo);

            if (cliente == null)
                throw new InvalidOperationException("Cliente no encontrado.");

            var telefono = model.Telefono.Replace(" ", string.Empty).Trim();
            var email = model.Email.Trim().ToLowerInvariant();
            var nombreCompleto = $"{model.Nombres.Trim()} {model.Apellidos.Trim()}".Trim();

            if (string.IsNullOrWhiteSpace(nombreCompleto))
                throw new InvalidOperationException("Ingresa nombres y apellidos.");

            if (string.IsNullOrWhiteSpace(telefono))
                throw new InvalidOperationException("El teléfono es obligatorio.");

            var emailDuplicado = await _databaseService.Cliente.AnyAsync(c =>
                c.Estado == true &&
                c.ClienteID != clienteId &&
                (c.Email == email || c.UserNamePortal == email));

            if (emailDuplicado)
                throw new InvalidOperationException("Ya existe otro cliente con ese correo.");

            var telefonoDuplicado = await _databaseService.Cliente.AnyAsync(c =>
                c.Estado == true &&
                c.ClienteID != clienteId &&
                (c.Telefono == telefono || c.TelefonoSecundario == telefono));

            if (telefonoDuplicado)
                throw new InvalidOperationException("Ya existe otro cliente con ese teléfono.");

            cliente.NombreCompleto = nombreCompleto;
            cliente.Telefono = telefono;
            cliente.Email = email;
            cliente.UserNamePortal = email;
            cliente.UsuarioModificacion = email;
            cliente.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();

            model.ClienteID = cliente.ClienteID;
            model.NombreCompleto = nombreCompleto;
            model.NumeroDocumento = cliente.NumeroDocumento;
            model.Email = email;
            return model;
        }
    }
}
