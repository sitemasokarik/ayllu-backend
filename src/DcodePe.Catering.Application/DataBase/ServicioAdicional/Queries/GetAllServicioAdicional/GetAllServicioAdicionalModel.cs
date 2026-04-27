using System;
using System.Collections.Generic;

namespace DcodePe.Catering.Application.DataBase.ServicioAdicional.Queries.GetAllServicioAdicional
{
    public class GetAllServicioAdicionalModel
    {
        public int ServicioID { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public decimal Precio { get; set; }
        
        public List<string> FotosUrls { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsuarioEliminacion { get; set; }

        public DateTime? FechaEliminacion { get; set; }

        public bool? Estado { get; set; }
    }
}
