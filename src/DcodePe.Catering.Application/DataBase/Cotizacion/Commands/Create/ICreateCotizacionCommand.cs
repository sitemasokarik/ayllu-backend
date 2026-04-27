using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Create
{
    public interface ICreateCotizacionCommand
    {
        Task<CreateCotizacionModel> ExecuteSaveCotizacion(CreateCotizacionModel model);

    }
}
