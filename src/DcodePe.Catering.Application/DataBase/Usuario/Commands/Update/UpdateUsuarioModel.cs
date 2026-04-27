namespace DcodePe.Catering.Application.DataBase.Usuario.Commands.Update
{
    public class UpdateUsuarioModel
    {
        public int UsuarioID { get; set; }
        
        public string Nombre { get; set; }
        
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        public int RolID { get; set; }
        
        public string UsuarioModificacion { get; set; }
    }
}
