using DcodePe.Catering.Domain.Entities.Facturacion;

namespace DcodePe.Catering.Application.External.Sunat
{
    public interface ISunatEmisionService
    {
        Task<SunatEmisionResult> EmitirComprobanteAsync(ComprobanteElectronicoEntity comprobante);
    }
}
