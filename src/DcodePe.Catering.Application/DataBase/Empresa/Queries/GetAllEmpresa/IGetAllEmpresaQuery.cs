using System.Collections.Generic;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Empresa.Queries.GetAllEmpresa
{
    public interface IGetAllEmpresaQuery
    {
        Task<List<GetAllEmpresaModel>> ExecuteListEmpresa();
        Task<GetAllEmpresaModel> ExecuteGetEmpresaById(int empresaId);
        Task<GetAllEmpresaModel> ExecuteGetEmpresaActiva();
        Task<EmpresaFacturacionConfigModel?> ExecuteGetFacturacionConfig(bool sunatIntegrado);
    }
}
