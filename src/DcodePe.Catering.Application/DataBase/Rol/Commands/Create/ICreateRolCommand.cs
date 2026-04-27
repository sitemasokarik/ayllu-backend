namespace DcodePe.Catering.Application.DataBase.Rol.Commands.Create
{
    public interface ICreateRolCommand
    {
        Task<CreateRolModel> Execute(CreateRolModel model);
    }
}
