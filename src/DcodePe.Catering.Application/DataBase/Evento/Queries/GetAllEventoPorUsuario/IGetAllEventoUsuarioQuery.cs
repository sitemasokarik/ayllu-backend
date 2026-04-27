using DcodePe.Catering.Application.DataBase.Evento.Queries.GetAllEventoPorUsuario;

namespace DcodePe.Catering.Application.DataBase.Evento.Queries.GetAllEventoUsuario
{
    public interface IGetAllEventoUsuarioQuery
    {
        Task<List<GetAllEventoUsuarioModel>> ExecuteListaEventoPorUsuario(int usuarioId);
    }
}
