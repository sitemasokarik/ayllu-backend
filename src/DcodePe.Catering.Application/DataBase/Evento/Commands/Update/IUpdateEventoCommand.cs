namespace DcodePe.Catering.Application.DataBase.Evento.Commands.Update
{
    public interface IUpdateEventoCommand
    {
        Task<bool> Execute(UpdateEventoModel model);
    }
}
