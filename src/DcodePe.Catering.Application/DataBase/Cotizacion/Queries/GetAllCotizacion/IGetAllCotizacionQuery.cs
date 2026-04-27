using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetAllCotizacion
{
    public interface IGetAllCotizacionQuery
    {
        Task<List<GetAllCotizacionModel>> ExecuteListaCotizacion();

        Task<List<GetAllCotizacionModel>> ExecuteListaCotizacionById(int CotizacionID);
    }
}
