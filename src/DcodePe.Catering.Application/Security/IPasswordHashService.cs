namespace DcodePe.Catering.Application.Security
{
    public interface IPasswordHashService
    {
        /// <summary>
        /// Genera un hash seguro de la contraseña usando BCrypt
        /// </summary>
        string HashPassword(string password);

        /// <summary>
        /// Verifica si la contraseña en texto plano coincide con el hash
        /// </summary>
        bool VerifyPassword(string password, string hashedPassword);
    }
}
