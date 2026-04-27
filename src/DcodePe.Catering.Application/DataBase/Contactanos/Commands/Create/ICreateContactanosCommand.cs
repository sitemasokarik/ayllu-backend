using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Contactanos.Commands.Create
{
    public interface ICreateContactanosCommand
    {
        Task<CreateContactanosModel> Execute(CreateContactanosModel model);
    }
}
