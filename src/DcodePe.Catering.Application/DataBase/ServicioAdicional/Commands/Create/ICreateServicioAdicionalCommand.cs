namespace DcodePe.Catering.Application.DataBase.ServicioAdicional.Commands.Create
{
    public interface ICreateServicioAdicionalCommand
    {
        Task<CreateServicioAdicionalModel> ExecuteSaveServicioAdicional(CreateServicioAdicionalModel model);
    }
}
