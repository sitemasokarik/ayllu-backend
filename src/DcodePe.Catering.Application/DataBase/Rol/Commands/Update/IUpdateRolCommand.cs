namespace DcodePe.Catering.Application.DataBase.Rol.Commands.Update
{
    public interface IUpdateRolCommand
    {
        Task<bool> Execute(UpdateRolModel model);
    }
}
