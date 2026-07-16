using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Evento.Queries.GetById
{
    public class GetEventoByIdQuery : IGetEventoByIdQuery
    {
        private readonly IDataBaseService _databaseService;

        public GetEventoByIdQuery(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<GetEventoByIdModel> Execute(int eventoId)
        {
            var evento = await _databaseService.Evento
                .Where(e => e.EventoID == eventoId && e.Estado == true)
                .Select(e => new GetEventoByIdModel
                {
                    EventoID = e.EventoID,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    FotosUrls = string.IsNullOrEmpty(e.Fotos)
                        ? new List<string>()
                        : e.Fotos.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    EstadoEvento = e.EstadoEvento,
                    TotalCotizaciones = e.Cotizaciones.Count(c => c.Estado == true),
                    UsuarioCreacion = e.UsuarioCreacion,
                    FechaCreacion = e.FechaCreacion,
                    UsuarioModificacion = e.UsuarioModificacion,
                    FechaModificacion = e.FechaModificacion,
                    Estado = e.Estado,
                    TarifasInvitadoJson = e.TarifasInvitadoJson
                })
                .FirstOrDefaultAsync();

            return evento;
        }
    }
}
