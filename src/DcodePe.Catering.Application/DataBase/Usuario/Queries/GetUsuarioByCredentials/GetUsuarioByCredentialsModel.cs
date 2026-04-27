namespace DcodePe.Catering.Application.DataBase.Usuario.Queries.GetUsuarioByCredentials
{
    public class GetUsuarioByCredentialsModel
    {
        public int UsuarioID { get; set; }
        
        public string Nombre { get; set; }
        
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        public int RolID { get; set; }
        
        public string RolNombre { get; set; }
        
        public string Token { get; set; }
        
        //public DateTime? FechaCreacion { get; set; }
    }
}
