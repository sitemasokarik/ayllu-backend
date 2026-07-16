using DcodePe.Catering.Application.Helpers;

namespace DcodePe.Catering.Application.DataBase.Cliente.Queries.GetAllCliente
{
    public class GetAllClienteQuery : IGetAllClienteQuery
    {
        private readonly IDataBaseService _databaseService;

        public GetAllClienteQuery(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<GetAllClienteModel>> Execute()
        {
            var result = await _databaseService.Cliente
                .Where(c => c.Estado == true)
                .Select(c => new GetAllClienteModel
                {
                    ClienteID = c.ClienteID,
                    TipoDocumento = c.TipoDocumento,
                    NumeroDocumento = c.NumeroDocumento,
                    NombreCompleto = c.NombreCompleto,
                    Email = c.Email,
                    Telefono = c.Telefono,
                    TelefonoSecundario = c.TelefonoSecundario,
                    Direccion = c.Direccion,
                    Ciudad = c.Ciudad,
                    Pais = c.Pais,
                    TipoCliente = c.TipoCliente,
                    Observaciones = c.Observaciones,
                    EsVIP = c.EsVIP,
                    FechaNacimiento = c.FechaNacimiento,
                    UsuarioCreacion = c.UsuarioCreacion,
                    FechaCreacion = c.FechaCreacion,
                    UsuarioModificacion = c.UsuarioModificacion,
                    FechaModificacion = c.FechaModificacion,
                    Estado = c.Estado,
                    TotalCotizaciones = c.Cotizaciones.Count(cot => cot.Estado == true),
                    EsPortalActivo = c.EsPortalActivo,
                    UserNamePortal = c.UserNamePortal,
                    TieneCuentaPortal = c.EsPortalActivo && c.UserNamePortal != null && c.UserNamePortal != ""
                })
                .ToListAsync();

            return result;
        }

        public async Task<GetAllClienteModel> GetByNumeroDocumento(string numeroDocumento)
        {
            var dni = DocumentoHelper.NormalizarDni(numeroDocumento);
            var result = await _databaseService.Cliente
                .Where(c => c.Estado == true &&
                    (c.NumeroDocumento == numeroDocumento || c.NumeroDocumento == dni))
                .Select(c => new GetAllClienteModel
                {
                    ClienteID = c.ClienteID,
                    TipoDocumento = c.TipoDocumento,
                    NumeroDocumento = c.NumeroDocumento,
                    NombreCompleto = c.NombreCompleto,
                    Email = c.Email,
                    Telefono = c.Telefono,
                    TelefonoSecundario = c.TelefonoSecundario,
                    Direccion = c.Direccion,
                    Ciudad = c.Ciudad,
                    Pais = c.Pais,
                    TipoCliente = c.TipoCliente,
                    Observaciones = c.Observaciones,
                    EsVIP = c.EsVIP,
                    FechaNacimiento = c.FechaNacimiento,
                    UsuarioCreacion = c.UsuarioCreacion,
                    FechaCreacion = c.FechaCreacion,
                    UsuarioModificacion = c.UsuarioModificacion,
                    FechaModificacion = c.FechaModificacion,
                    Estado = c.Estado,
                    TotalCotizaciones = c.Cotizaciones.Count(cot => cot.Estado == true),
                    EsPortalActivo = c.EsPortalActivo,
                    UserNamePortal = c.UserNamePortal,
                    TieneCuentaPortal = c.EsPortalActivo && c.UserNamePortal != null && c.UserNamePortal != ""
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
