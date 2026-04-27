using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.ServicioAdicional.Commands.Update
{
    public interface IUpdateServicioAdicionalCommand
    {
        Task<bool> Execute(UpdateServicioAdicionalModel model);
    }
}
