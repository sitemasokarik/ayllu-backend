using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.PaqueteServicio.Commands.Create
{
    public class CreatePaqueteServicioModel
    {
        public int PaqueteID { get; set; }

        public int ServicioID { get; set; }
        public string UsuarioCreacion { get; set; }

    }
}
