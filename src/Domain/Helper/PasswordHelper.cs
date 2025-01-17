using BC = BCrypt.Net;

namespace Domain.Helper
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password) => BC.BCrypt.HashPassword(password);
        public static bool VerifyPassword(string password, string hashedPassword) => BC.BCrypt.Verify(password, hashedPassword);
    }
}
