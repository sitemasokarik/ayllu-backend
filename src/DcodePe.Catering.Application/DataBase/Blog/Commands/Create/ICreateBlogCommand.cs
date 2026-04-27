namespace DcodePe.Catering.Application.DataBase.Blog.Commands.Create
{
    public interface ICreateBlogCommand
    {
        Task<CreateBlogModel> Execute(CreateBlogModel model);
    }
}
