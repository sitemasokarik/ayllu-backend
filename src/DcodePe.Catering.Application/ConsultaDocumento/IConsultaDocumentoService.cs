namespace DcodePe.Catering.Application.ConsultaDocumento
{
    public interface IConsultaDocumentoService
    {
        Task<ConsultaDocumentoResult> ValidateRucAsync(string numero);
        Task<ConsultaDocumentoResult> ValidateDniAsync(string numero);
    }
}
