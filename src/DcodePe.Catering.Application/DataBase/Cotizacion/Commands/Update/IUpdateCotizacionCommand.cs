using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Update
{
    public interface IUpdateCotizacionCommand
    {
        Task<bool> Execute(UpdateCotizacionModel model);
    }
}
