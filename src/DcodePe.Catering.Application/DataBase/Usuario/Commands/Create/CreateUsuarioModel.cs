namespace DcodePe.Catering.Application.DataBase.Usuario.Commands.Create
{
    public class CreateUsuarioModel
    {
        public string Nombre { get; set; }
        
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public int RolID { get; set; }
        
        public string UsuarioCreacion { get; set; }
    }
}
