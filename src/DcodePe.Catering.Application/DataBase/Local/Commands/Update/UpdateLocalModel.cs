using System.Collections.Generic;

namespace DcodePe.Catering.Application.DataBase.Local.Commands.Update
{
    public class UpdateLocalModel
    {
        public int LocalID { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int Capacidad { get; set; }
        public decimal PrecioAlquiler { get; set; }
        public decimal HorasEvento { get; set; }
        //public string Fotos { get; set; }
        public List<string> FotosUrls { get; set; }
        public string TerminosCondiciones { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
