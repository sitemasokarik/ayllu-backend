namespace DcodePe.Catering.Application.DataBase.Evento.Commands.Update
{
    public class UpdateEventoModel
    {
        public int EventoID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        
        /// <summary>
        /// URL de la foto del evento
        /// </summary>
        public string Fotos { get; set; }
        
        public string EstadoEvento { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
