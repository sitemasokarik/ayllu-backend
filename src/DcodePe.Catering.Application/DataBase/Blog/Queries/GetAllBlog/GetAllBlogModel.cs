using System;
using System.Collections.Generic;
using DcodePe.Catering.Application.DataBase.Blog.Models;

namespace DcodePe.Catering.Application.DataBase.Blog.Queries.GetAllBlog
{
    public class GetAllBlogModel
    {
        public int BlogID { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public List<ValorModel> Valores { get; set; }
        //public string Imagenes { get; set; }
        public List<string> ImagenesUrls { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioEliminacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public bool? Estado { get; set; }
    }
}
