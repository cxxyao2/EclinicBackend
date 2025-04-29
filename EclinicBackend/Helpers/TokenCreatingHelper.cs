using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
using Microsoft.IdentityModel.Tokens;

namespace EclinicBackend.Helpers
{
    internal static class TokenCreatingHelper
    {

        public static string CreateToken(User user, string tokenSecret)
        {


            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.UserID.ToString()),
                new Claim(ClaimTypes.Name,user.UserName)
            };

            SymmetricSecurityKey key = new(System.Text.Encoding.UTF8.GetBytes(tokenSecret));

            SigningCredentials cred = new(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: cred
                );

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }


        public static RefreshToken GenerateRefreshToken()
        {
            RefreshToken refreshToken = new()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };

            return refreshToken;
        }
    }
}
