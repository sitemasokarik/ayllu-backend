namespace DcodePe.Catering.Application.DataBase.Usuario.Commands.Create
{
    public class CreateUsuarioResponseModel
    {
        public int UsuarioID { get; set; }
        
        public string Nombre { get; set; }
        
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        public int RolID { get; set; }
        
        public DateTime? FechaCreacion { get; set; }
    }
}
