using DcodePe.Catering.Application.DataBase.Blog;
using DcodePe.Catering.Application.DataBase.Blog.Models;

namespace DcodePe.Catering.Application.DataBase.Blog.Queries.GetAllBlog
{
    public class GetAllBlogQuery : IGetAllBlogQuery
    {
        private readonly IDataBaseService _databaseService;

        public GetAllBlogQuery(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<GetAllBlogModel>> ExecuteListBlog()
        {
            var blogs = await _databaseService.Blog
                .Where(b => b.Estado == true)
                .ToListAsync();

            return blogs.Select(MapBlog).ToList();
        }

        public async Task<GetAllBlogModel> ExecuteGetBlogById(int blogId)
        {
            var blog = await _databaseService.Blog
                .Where(b => b.BlogID == blogId && b.Estado==true)
                .FirstOrDefaultAsync();

            if (blog == null)
                return null;

            return MapBlog(blog);
        }

        private static GetAllBlogModel MapBlog(Domain.Entities.BlogEntity blog)
        {
            return new GetAllBlogModel
            {
                BlogID = blog.BlogID,
                Titulo = blog.Titulo,
                Descripcion = blog.Descripcion,
                Resumen = blog.Resumen,
                MisionTitulo = blog.MisionTitulo,
                MisionTexto = blog.MisionTexto,
                VisionTitulo = blog.VisionTitulo,
                VisionTexto = blog.VisionTexto,
                ImagenesUrls = blog.ImagenesUrls?.ToList() ?? new List<string>(),
                LandingConfig = LandingConfigMapper.FromEntity(blog),
                Valores = blog.Valores?.Select(v => new ValorModel
                {
                    Nombre = v.Nombre,
                    Descripcion = v.Descripcion
                }).ToList() ?? new List<ValorModel>(),
                UsuarioCreacion = blog.UsuarioCreacion,
                FechaCreacion = blog.FechaCreacion,
                UsuarioModificacion = blog.UsuarioModificacion,
                FechaModificacion = blog.FechaModificacion,
                UsuarioEliminacion = blog.UsuarioEliminacion,
                FechaEliminacion = blog.FechaEliminacion,
                Estado = blog.Estado
            };
        }
    }
}
