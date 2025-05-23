using BCrypt.Net;

namespace Backend_PotenciaPC.Utilities
{
    public class Encrypt
    {
        // Hashea una contraseña usando bcrypt
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Verifica una contraseña contra un hash almacenado
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
