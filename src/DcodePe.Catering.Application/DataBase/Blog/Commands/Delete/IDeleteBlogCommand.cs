using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Blog.Commands.Delete
{
    public interface IDeleteBlogCommand
    {
        Task<bool> Execute(int blogId, string usuarioEliminacion);
    }
}
