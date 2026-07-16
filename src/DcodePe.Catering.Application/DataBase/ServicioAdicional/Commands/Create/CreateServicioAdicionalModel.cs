using System.Collections.Generic;

namespace DcodePe.Catering.Application.DataBase.ServicioAdicional.Commands.Create
{
    public class CreateServicioAdicionalModel
    {
        public int ServicioID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int CantidadMinima { get; set; } = 1;
        public List<string> FotosUrls { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Estado { get; set; }
    }
}
