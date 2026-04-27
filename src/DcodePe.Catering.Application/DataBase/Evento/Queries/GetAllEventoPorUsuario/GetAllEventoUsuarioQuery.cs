using DcodePe.Catering.Application.DataBase.Evento.Queries.GetAllEventoUsuario;

namespace DcodePe.Catering.Application.DataBase.Evento.Queries.GetAllEventoPorUsuario
{
    public class GetAllEventoUsuarioQuery(IDataBaseService databaseService) : IGetAllEventoUsuarioQuery
    {
        private readonly IDataBaseService _databaseService = databaseService;
        
        public async Task<List<GetAllEventoUsuarioModel>> ExecuteListaEventoPorUsuario(int usuarioId)
        {
            // Esta query ya no es válida con la nueva relación Evento-Cotizacion
            // Se debe usar una query que busque eventos por CotizacionID
            // Retornando lista vacía por compatibilidad
            return new List<GetAllEventoUsuarioModel>();
        }
    }
}
