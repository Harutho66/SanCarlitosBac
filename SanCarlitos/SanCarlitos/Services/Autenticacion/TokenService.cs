using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SanCarlitos.Configuration;
using SanCarlitos.Dtos.Autenticacion;
using SanCarlitos.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SanCarlitos.Services.Autenticacion
{

    public sealed class TokenService : ITokenService
    {
        private readonly JwtSettings _settings;

        public TokenService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;

            if (string.IsNullOrWhiteSpace(_settings.Issuer))
                throw new InvalidOperationException(
                    "La configuración Jwt:Issuer es obligatoria.");

            if (string.IsNullOrWhiteSpace(_settings.Audience))
                throw new InvalidOperationException(
                    "La configuración Jwt:Audience es obligatoria.");

            if (string.IsNullOrWhiteSpace(_settings.SecretKey) ||
                Encoding.UTF8.GetByteCount(_settings.SecretKey) < 32)
            {
                throw new InvalidOperationException(
                    "Jwt:SecretKey debe tener al menos 32 bytes UTF-8.");
            }

            if (_settings.ExpirationMinutes <= 0)
                throw new InvalidOperationException(
                    "Jwt:ExpirationMinutes debe ser mayor que cero.");
        }

        public TokenDto GenerarToken(Usuario usuario)
        {
            var ahora = DateTime.UtcNow;
            var expiracion = ahora.AddMinutes(
                _settings.ExpirationMinutes);

            var claims = new List<Claim>
        {
            new(
                JwtRegisteredClaimNames.Sub,
                usuario.Id.ToString()),

            new(
                ClaimTypes.NameIdentifier,
                usuario.Id.ToString()),

            new(
                ClaimTypes.Name,
                usuario.NombreUsuario),

            new(
                ClaimTypes.Role,
                usuario.Rol),

            new(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

            var clave = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_settings.SecretKey));

            var credenciales = new SigningCredentials(
                clave,
                SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                notBefore: ahora,
                expires: expiracion,
                signingCredentials: credenciales);

            var token = new JwtSecurityTokenHandler()
                .WriteToken(jwt);

            return new TokenDto
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresAtUtc = expiracion,
                UsuarioId = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Rol = usuario.Rol
            };
        }
    }
}
