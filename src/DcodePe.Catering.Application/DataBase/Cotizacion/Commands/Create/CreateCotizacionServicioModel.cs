using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Create
{
    public class CreateCotizacionServicioModel
    {
        public int CotizacionID { get; set; }
        public int ServicioID { get; set; }
        public int Cantidad { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Estado { get; set; }

    }
}
