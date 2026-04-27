using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Empresa.Commands.Create
{
    public interface ICreateEmpresaCommand
    {
        Task<CreateEmpresaModel> Execute(CreateEmpresaModel model);
    }
}
