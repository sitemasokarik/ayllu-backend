namespace DcodePe.Catering.Application.DataBase.PaqueteServicio.Commands.Create
{
    public interface ICreatePaqueteServicioCommand
    {
        Task<CreatePaqueteServicioModel> CreatePaqueteServicio(CreatePaqueteServicioModel model);
    }
}
