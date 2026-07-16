namespace DcodePe.Catering.Application.DataBase.Comprobante.Queries.GetAllComprobante
{
    public interface IGetAllComprobanteQuery
    {
        Task<List<GetAllComprobanteModel>> Execute(string? tipo = null);
    }
}
