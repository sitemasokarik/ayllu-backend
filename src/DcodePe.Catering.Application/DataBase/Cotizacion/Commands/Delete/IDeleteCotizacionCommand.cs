using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Delete
{
    public interface IDeleteCotizacionCommand
    {
        Task<bool> Execute(int cotizacionId, string usuarioEliminacion);
    }
}
