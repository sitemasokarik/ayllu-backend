namespace DcodePe.Catering.Application.DataBase.Usuario.Queries.GetUsuarioById
{
    public class GetUsuarioByIdModel
    {
        public int UsuarioID { get; set; }
        
        public string Nombre { get; set; }
        
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        public int RolID { get; set; }
        
        public string RolNombre { get; set; }
        
        public string UsuarioCreacion { get; set; }
        
        public DateTime? FechaCreacion { get; set; }
        
        public string UsuarioModificacion { get; set; }
        
        public DateTime? FechaModificacion { get; set; }
        
        public bool? Estado { get; set; }
    }
}
