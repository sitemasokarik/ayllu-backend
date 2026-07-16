using System;
using System.Collections.Generic;

using DcodePe.Catering.Application.DataBase.Evento.Queries.GetAllEvento;

namespace DcodePe.Catering.Application.DataBase.Local.Queries.GetAllLocal
{
    public class GetAllLocalModel
    {
        public int LocalID { get; set; }

        public string Nombre { get; set; }

        public string Direccion { get; set; }

        public int Capacidad { get; set; }

        public decimal PrecioAlquiler { get; set; }

        public decimal Garantia { get; set; }

        public decimal HorasEvento { get; set; }

        //public string Fotos { get; set; }

        public List<string> FotosUrls { get; set; }

        public string TerminosCondiciones { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsuarioEliminacion { get; set; }

        public DateTime? FechaEliminacion { get; set; }

        public bool? Estado { get; set; }


    }
}
