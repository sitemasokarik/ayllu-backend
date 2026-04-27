using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Empresa.Commands.Update
{
    public interface IUpdateEmpresaCommand
    {
        Task<bool> Execute(UpdateEmpresaModel model);
    }
}
