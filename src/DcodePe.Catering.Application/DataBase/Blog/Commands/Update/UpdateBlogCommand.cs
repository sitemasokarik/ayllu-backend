using DcodePe.Catering.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Blog.Commands.Update
{
    public class UpdateBlogCommand : IUpdateBlogCommand
    {
        private readonly IDataBaseService _databaseService;

        public UpdateBlogCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(UpdateBlogModel model)
        {
            var entity = await _databaseService.Blog
                .FirstOrDefaultAsync(b => b.BlogID == model.BlogID);

            if (entity == null)
                return false;

            entity.Titulo = model.Titulo;
            entity.Descripcion = model.Descripcion;
            entity.ValoresJson = model.Valores != null && model.Valores.Any()
                ? JsonSerializer.Serialize(model.Valores.Select(v => new ValorEmpresarial
                {
                    Nombre = v.Nombre,
                    Descripcion = v.Descripcion
                }))
                : null;
            entity.Imagenes = model.ImagenesUrls != null && model.ImagenesUrls.Any()
                ? string.Join(";", model.ImagenesUrls)
                : model.Imagenes;
            entity.UsuarioModificacion = model.UsuarioModificacion ?? "SYSTEM";
            entity.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
