using DcodePe.Catering.Domain.Entities;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Blog.Commands.Create
{
    public class CreateBlogCommand : ICreateBlogCommand
    {
        private readonly IDataBaseService _databaseService;

        public CreateBlogCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<CreateBlogModel> Execute(CreateBlogModel model)
        {
            var entity = new BlogEntity
            {
                Titulo = model.Titulo,
                Descripcion = model.Descripcion,
                Resumen = model.Resumen,
                MisionTitulo = model.MisionTitulo,
                MisionTexto = model.MisionTexto,
                VisionTitulo = model.VisionTitulo,
                VisionTexto = model.VisionTexto,
                ValoresJson = model.Valores != null && model.Valores.Any()
                    ? JsonSerializer.Serialize(model.Valores.Select(v => new ValorEmpresarial
                    {
                        Nombre = v.Nombre,
                        Descripcion = v.Descripcion
                    }))
                    : null,
                Imagenes = model.ImagenesUrls != null && model.ImagenesUrls.Any()
                    ? string.Join(";", model.ImagenesUrls)
                    : "",
                UsuarioCreacion = model.UsuarioCreacion ?? "SYSTEM",
                FechaCreacion = DateTime.Now,
                Estado = model.Estado ?? true
            };

            await _databaseService.Blog.AddAsync(entity);
            await _databaseService.SaveAsync();

            model.BlogID = entity.BlogID;
            return model;
        }
    }
}
