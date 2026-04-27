namespace DcodePe.Catering.Application.DataBase.Local.Commands.Update
{
    public interface IUpdateLocalCommand
    {
        Task<bool> Execute(UpdateLocalModel model);
    }
}
