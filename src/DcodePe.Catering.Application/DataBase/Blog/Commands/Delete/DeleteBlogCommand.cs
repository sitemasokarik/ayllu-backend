using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Blog.Commands.Delete
{
    public class DeleteBlogCommand : IDeleteBlogCommand
    {
        private readonly IDataBaseService _databaseService;

        public DeleteBlogCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(int blogId, string usuarioEliminacion)
        {
            var entity = await _databaseService.Blog
                .FirstOrDefaultAsync(b => b.BlogID == blogId);

            if (entity == null)
                return false;

            entity.Estado = false;
            entity.UsuarioEliminacion = usuarioEliminacion ?? "SYSTEM";
            entity.FechaEliminacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
