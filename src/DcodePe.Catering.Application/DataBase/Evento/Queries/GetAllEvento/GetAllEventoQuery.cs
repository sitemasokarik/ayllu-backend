namespace DcodePe.Catering.Application.DataBase.Evento.Queries.GetAllEvento
{
    public class GetAllEventoQuery(IDataBaseService databaseService) : IGetAllEventoQuery
    {
        private readonly IDataBaseService _databaseService = databaseService;
        
        public async Task<List<GetAllEventoModel>> ExecuteListEvento()
        {
            var result = await _databaseService.Evento
                .Where(e => e.Estado == true)
                .AsNoTracking()
                .Select(evento => new GetAllEventoModel()
                {
                    EventoID = evento.EventoID,
                    Nombre = evento.Nombre,
                    Descripcion = evento.Descripcion,
                    Fotos = evento.Fotos,
                    EstadoEvento = evento.EstadoEvento,
                    UsuarioCreacion = evento.UsuarioCreacion,
                    FechaCreacion = evento.FechaCreacion,
                    UsuarioModificacion = evento.UsuarioModificacion,
                    FechaModificacion = evento.FechaModificacion,
                    UsuarioEliminacion = evento.UsuarioEliminacion,
                    FechaEliminacion = evento.FechaEliminacion,
                    Estado = evento.Estado
                }).ToListAsync();

            return result;
        }
    }
}
