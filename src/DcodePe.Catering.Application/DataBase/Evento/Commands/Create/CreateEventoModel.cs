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

        /// <summary>
        /// URL de la foto del evento
        /// </summary>
        public string Fotos { get; set; }

        public string EstadoEvento { get; set; }
    }
}
