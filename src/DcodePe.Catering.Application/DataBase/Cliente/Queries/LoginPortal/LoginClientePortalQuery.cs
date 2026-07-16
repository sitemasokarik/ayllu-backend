using DcodePe.Catering.Application.External.GetTokenJwt;
using DcodePe.Catering.Application.Security;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Cliente.Queries.LoginPortal
{
    public class LoginClientePortalQuery(
        IDataBaseService databaseService,
        IPasswordHashService passwordHashService,
        IGetTokenJwtService getTokenJwtService) : ILoginClientePortalQuery
    {
        private readonly IDataBaseService _databaseService = databaseService;
        private readonly IPasswordHashService _passwordHashService = passwordHashService;
        private readonly IGetTokenJwtService _getTokenJwtService = getTokenJwtService;

        public async Task<LoginClientePortalModel?> Execute(string email, string password)
        {
            var emailNormalizado = email.Trim().ToLowerInvariant();

            var cliente = await _databaseService.Cliente
                .FirstOrDefaultAsync(c =>
                    c.Estado == true &&
                    c.EsPortalActivo &&
                    (c.UserNamePortal == emailNormalizado || c.Email == emailNormalizado));

            if (cliente == null || string.IsNullOrWhiteSpace(cliente.PasswordHash))
                return null;

            var isPasswordValid = _passwordHashService.VerifyPassword(password, cliente.PasswordHash);

            if (!isPasswordValid)
                return null;

            return new LoginClientePortalModel
            {
                Token = _getTokenJwtService.Execute(cliente.ClienteID.ToString(), "cliente"),
                ClienteID = cliente.ClienteID,
                NombreCompleto = cliente.NombreCompleto,
                Email = cliente.Email ?? emailNormalizado,
                TipoDocumento = cliente.TipoDocumento,
                NumeroDocumento = cliente.NumeroDocumento,
                Telefono = cliente.Telefono
            };
        }
    }
}
