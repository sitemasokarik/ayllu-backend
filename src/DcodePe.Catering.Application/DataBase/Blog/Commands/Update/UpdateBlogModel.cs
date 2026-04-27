using System.Collections.Generic;
using DcodePe.Catering.Application.DataBase.Blog.Models;

namespace DcodePe.Catering.Application.DataBase.Blog.Commands.Update
{
    public class UpdateBlogModel
    {
        public int BlogID { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public List<ValorModel> Valores { get; set; }
        public string Imagenes { get; set; }
        public List<string> ImagenesUrls { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
