namespace DcodePe.Catering.Application.DataBase.Usuario.Queries.GetUsuarioById
{
    public interface IGetUsuarioByIdQuery
    {
        Task<GetUsuarioByIdModel> Execute(int usuarioId);
    }
}
