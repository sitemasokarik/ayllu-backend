namespace DcodePe.Catering.Application.DataBase.Contactanos.Commands.Create
{
    public class CreateContactanosModel
    {
        public int ContactanosID { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Servicio { get; set; }
        public string Mensaje { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Estado { get; set; }
    }
}
