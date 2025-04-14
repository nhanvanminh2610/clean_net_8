using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Entities.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Helper
{
    public static class TokenHelper
    {
        //store this payload and key in the appsetting/cfg file privately
        public const string Issuer = "https://localhost:5001";
        public const string Audience = "https://localhost:5001";
        public const string Secret = "hHr4Ly0Y5o54HB0acxRDMipIe0b60jfk";
        public static async Task<string> GenerateAccessToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Secret);

            var claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            });

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = Issuer,
                Audience = Audience,
                Expires = DateTime.Now.AddDays(60),
                SigningCredentials = signingCredentials,

            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return await Task.Run(() => tokenHandler.WriteToken(securityToken));
        }
        public static async Task<string> GenerateUserAccessToken(User user, IList<string>? roles = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Secret);

            var claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            });

            if (roles == null)
            {
                foreach (var role in user.UserRoles)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Role.Name));
                }
            }
            else
            {
                foreach (var role in roles)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                }
            }

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = Issuer,
                Audience = Audience,
                Expires = DateTime.Now.AddDays(60),
                SigningCredentials = signingCredentials,
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return await Task.Run(() => tokenHandler.WriteToken(securityToken));
        }
        public static async Task<string> GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[32];

            using var randomNumberGenerator = RandomNumberGenerator.Create();
            await Task.Run(() => randomNumberGenerator.GetBytes(secureRandomBytes));

            var refreshToken = Convert.ToBase64String(secureRandomBytes);
            return refreshToken;
        }
    }
}
