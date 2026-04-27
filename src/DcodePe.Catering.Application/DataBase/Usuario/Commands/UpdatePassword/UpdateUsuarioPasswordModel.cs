namespace DcodePe.Catering.Application.DataBase.Usuario.Commands.UpdatePassword
{
    public class UpdateUsuarioPasswordModel
    {
        public int UsuarioID { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
        
        public string ConfirmPassword { get; set; }
    }
}
