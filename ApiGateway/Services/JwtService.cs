using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ApiGateway.Services
{
    public class JwtService
    {
        private readonly string _key;
        private readonly int _expireMinutes;

        public JwtService(IConfiguration config)
        {
            _key = config["Jwt:Key"];
            _expireMinutes = int.Parse(config["Jwt:ExpireMinutes"] ?? "30"); // fallback a 30
        }

        public string GenerateToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(_expireMinutes),
                claims: claims,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
