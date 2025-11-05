using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SyndicApp.Application.DTOs.Auth;
using SyndicApp.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SyndicApp.Infrastructure.Identity
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _settings;

        public JwtTokenGenerator(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateToken(UserDto user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                // Identifiants utilisateur
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),   // ✅ compat ASP.NET
                new Claim("uid", user.Id.ToString()),                       // ✅ compat ancien code

                // Infos
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("FullName", user.FullName ?? string.Empty)
            };

            // Rôles
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
                claims: claims,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
