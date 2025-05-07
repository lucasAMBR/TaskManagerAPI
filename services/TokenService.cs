using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Services{
    public static class TokenService{
        public static string GenerateToken(string userId){
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId) 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("chave-muito-super-secreta-temporaria-12345"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}