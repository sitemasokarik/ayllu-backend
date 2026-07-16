using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Evento.Commands.Create
{
    public class CreateEventoModel
    {
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public List<string> FotosUrls { get; set; }

        public string EstadoEvento { get; set; }
    }
}
