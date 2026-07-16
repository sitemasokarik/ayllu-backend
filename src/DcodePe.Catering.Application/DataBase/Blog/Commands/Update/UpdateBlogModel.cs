using System.Collections.Generic;
using DcodePe.Catering.Application.DataBase.Blog.Models;

namespace DcodePe.Catering.Application.DataBase.Blog.Commands.Update
{
    public class UpdateBlogModel
    {
        public int BlogID { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Resumen { get; set; }
        public string MisionTitulo { get; set; }
        public string MisionTexto { get; set; }
        public string VisionTitulo { get; set; }
        public string VisionTexto { get; set; }
        public List<ValorModel> Valores { get; set; }
        public List<string> ImagenesUrls { get; set; }
        public LandingConfigModel LandingConfig { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
