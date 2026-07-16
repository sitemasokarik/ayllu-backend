namespace DcodePe.Catering.Application.DataBase.Comprobante.Commands.Create
{
    public interface ICreateComprobanteCommand
    {
        Task<CreateComprobanteModel> Execute(CreateComprobanteModel model);
    }
}
