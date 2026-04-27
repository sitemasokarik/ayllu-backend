using BCrypt.Net;

namespace DcodePe.Catering.Application.Security
{
    public class PasswordHashService : IPasswordHashService
    {
        /// <summary>
        /// Genera un hash seguro de la contraseña usando BCrypt con workFactor 12
        /// </summary>
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña no puede estar vacía", nameof(password));

            // WorkFactor 12 proporciona un buen balance entre seguridad y rendimiento
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        /// <summary>
        /// Verifica si la contraseña en texto plano coincide con el hash BCrypt
        /// </summary>
        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (string.IsNullOrWhiteSpace(hashedPassword))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch
            {
                // Si el hash no es válido o hay algún error, retorna false
                return false;
            }
        }
    }
}
