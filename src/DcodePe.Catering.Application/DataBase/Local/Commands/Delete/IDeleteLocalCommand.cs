namespace DcodePe.Catering.Application.DataBase.Local.Commands.Delete
{
    public interface IDeleteLocalCommand
    {
        Task<bool> Execute(int localId, string usuarioEliminacion);
    }
}
