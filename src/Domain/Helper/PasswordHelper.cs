using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using BC = BCrypt.Net;

namespace Domain.Helper
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password) => BC.BCrypt.HashPassword(password);
        public static bool VerifyPassword(string password, string hashedPassword) => BC.BCrypt.Verify(password, hashedPassword);

        public static byte[] GetSecureSalt()
        {
            return RandomNumberGenerator.GetBytes(32);
        }
        public static string HashUsingPbkdf2(string password, byte[] salt)
        {
            byte[] derivedKey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, iterationCount: 100000, 32);

            return Convert.ToBase64String(derivedKey);
        }
    }
}
