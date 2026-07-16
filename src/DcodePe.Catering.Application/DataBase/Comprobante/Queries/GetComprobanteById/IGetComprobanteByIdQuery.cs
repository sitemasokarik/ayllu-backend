using DcodePe.Catering.Application.DataBase.Comprobante.Queries.GetAllComprobante;

namespace DcodePe.Catering.Application.DataBase.Comprobante.Queries.GetComprobanteById
{
    public interface IGetComprobanteByIdQuery
    {
        Task<GetAllComprobanteModel?> Execute(int comprobanteId);
    }
}
