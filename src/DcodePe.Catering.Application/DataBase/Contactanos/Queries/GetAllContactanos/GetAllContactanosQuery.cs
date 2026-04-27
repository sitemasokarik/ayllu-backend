using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Contactanos.Queries.GetAllContactanos
{
    public class GetAllContactanosQuery : IGetAllContactanosQuery
    {
        private readonly IDataBaseService _databaseService;

        public GetAllContactanosQuery(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<GetAllContactanosModel>> ExecuteListContactanos()
        {
            var result = await _databaseService.Contactanos
                .Where(c => c.Estado == true)
                .OrderByDescending(c => c.FechaCreacion)
                .Select(contactanos => new GetAllContactanosModel
                {
                    ContactanosID = contactanos.ContactanosID,
                    NombreCompleto = contactanos.NombreCompleto,
                    Correo = contactanos.Correo,
                    Telefono = contactanos.Telefono,
                    Servicio = contactanos.Servicio,
                    Mensaje = contactanos.Mensaje,
                    UsuarioCreacion = contactanos.UsuarioCreacion,
                    FechaCreacion = contactanos.FechaCreacion,
                    UsuarioModificacion = contactanos.UsuarioModificacion,
                    FechaModificacion = contactanos.FechaModificacion,
                    UsuarioEliminacion = contactanos.UsuarioEliminacion,
                    FechaEliminacion = contactanos.FechaEliminacion,
                    Estado = contactanos.Estado
                })
                .ToListAsync();

            return result;
        }

        public async Task<GetAllContactanosModel> ExecuteGetContactanosById(int contactanosId)
        {
            var contactanos = await _databaseService.Contactanos
                .Where(c => c.ContactanosID == contactanosId)
                .FirstOrDefaultAsync();

            if (contactanos == null)
                return null;

            return new GetAllContactanosModel
            {
                ContactanosID = contactanos.ContactanosID,
                NombreCompleto = contactanos.NombreCompleto,
                Correo = contactanos.Correo,
                Telefono = contactanos.Telefono,
                Servicio = contactanos.Servicio,
                Mensaje = contactanos.Mensaje,
                UsuarioCreacion = contactanos.UsuarioCreacion,
                FechaCreacion = contactanos.FechaCreacion,
                UsuarioModificacion = contactanos.UsuarioModificacion,
                FechaModificacion = contactanos.FechaModificacion,
                UsuarioEliminacion = contactanos.UsuarioEliminacion,
                FechaEliminacion = contactanos.FechaEliminacion,
                Estado = contactanos.Estado
            };
        }
    }
}
