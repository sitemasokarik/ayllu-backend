namespace DcodePe.Catering.Application.DataBase.Usuario.Commands.UpdatePassword
{
    public interface IUpdateUsuarioPasswordCommand
    {
        Task<bool> Execute(UpdateUsuarioPasswordModel model);
    }
}
