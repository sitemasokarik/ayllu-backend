using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.ServicioAdicional.Queries.GetAllServicioAdicional
{
    public interface IGetServicioAdicionalQuery
    {
        Task<List<GetAllServicioAdicionalModel>> GetAllServicioAdicionalAsync();
        Task<GetAllServicioAdicionalModel> GetServicioAdicionalByIdAsync(int servicioId);
    }
}
