namespace DcodePe.Catering.Application.DataBase.Usuario.Commands.Update
{
    public interface IUpdateUsuarioCommand
    {
        Task<bool> Execute(UpdateUsuarioModel model);
    }
}
