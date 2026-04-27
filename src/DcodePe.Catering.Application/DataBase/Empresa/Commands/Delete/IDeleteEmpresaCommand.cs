using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Empresa.Commands.Delete
{
    public interface IDeleteEmpresaCommand
    {
        Task<bool> Execute(int empresaId, string usuarioEliminacion);
    }
}
