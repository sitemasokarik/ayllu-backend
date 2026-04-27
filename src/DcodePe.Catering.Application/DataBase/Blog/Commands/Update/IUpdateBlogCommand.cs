using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Blog.Commands.Update
{
    public interface IUpdateBlogCommand
    {
        Task<bool> Execute(UpdateBlogModel model);
    }
}
