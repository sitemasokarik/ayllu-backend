namespace DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetFechasReservadasLocal
{
    public interface IGetFechasReservadasLocalQuery
    {
        Task<List<string>> Execute(int localId);
    }

    public class GetFechasReservadasLocalQuery(IDataBaseService databaseService) : IGetFechasReservadasLocalQuery
    {
        private readonly IDataBaseService _databaseService = databaseService;

        public Task<List<string>> Execute(int localId)
            => Helpers.FechaReservadaHelper.GetFechasReservadasAsync(_databaseService, localId);
    }
}
