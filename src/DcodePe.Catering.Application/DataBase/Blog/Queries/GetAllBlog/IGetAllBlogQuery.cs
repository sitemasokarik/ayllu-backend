using System.Collections.Generic;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Blog.Queries.GetAllBlog
{
    public interface IGetAllBlogQuery
    {
        Task<List<GetAllBlogModel>> ExecuteListBlog();
        Task<GetAllBlogModel> ExecuteGetBlogById(int blogId);
    }
}
