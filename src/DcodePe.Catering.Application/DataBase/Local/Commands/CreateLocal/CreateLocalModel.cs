using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Local.Commands.CreateLocal
{
    public class CreateLocalModel
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int Capacidad { get; set; }
        public decimal PrecioAlquiler { get; set; }
        public decimal HorasEvento { get; set; }
        //public string Fotos { get; set; }
        public List<string> FotosUrls { get; set; }
        public string TerminosCondiciones { get; set; }
        public bool? Estado { get; set; }
    }
}
