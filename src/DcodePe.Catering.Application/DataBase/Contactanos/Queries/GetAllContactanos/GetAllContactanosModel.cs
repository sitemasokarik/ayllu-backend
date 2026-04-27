using System;

namespace DcodePe.Catering.Application.DataBase.Contactanos.Queries.GetAllContactanos
{
    public class GetAllContactanosModel
    {
        public int ContactanosID { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Servicio { get; set; }
        public string Mensaje { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioEliminacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public bool? Estado { get; set; }
    }
}
