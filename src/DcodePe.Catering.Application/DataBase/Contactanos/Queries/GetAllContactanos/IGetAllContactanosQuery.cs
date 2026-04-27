using System.Collections.Generic;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Contactanos.Queries.GetAllContactanos
{
    public interface IGetAllContactanosQuery
    {
        Task<List<GetAllContactanosModel>> ExecuteListContactanos();
        Task<GetAllContactanosModel> ExecuteGetContactanosById(int contactanosId);
    }
}
