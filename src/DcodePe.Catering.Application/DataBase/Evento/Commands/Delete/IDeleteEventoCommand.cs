namespace DcodePe.Catering.Application.DataBase.Evento.Commands.Delete
{
    public interface IDeleteEventoCommand
    {
        Task<bool> Execute(int eventoId, string usuarioEliminacion);
    }
}
