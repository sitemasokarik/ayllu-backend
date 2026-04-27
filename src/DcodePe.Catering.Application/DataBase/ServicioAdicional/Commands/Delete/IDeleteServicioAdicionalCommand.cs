using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.ServicioAdicional.Commands.Delete
{
    public interface IDeleteServicioAdicionalCommand
    {
        Task<bool> Execute(int servicioId, string usuarioEliminacion);
    }
}
