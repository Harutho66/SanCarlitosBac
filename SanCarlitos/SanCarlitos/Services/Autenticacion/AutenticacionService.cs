using Microsoft.AspNetCore.Identity;
using SanCarlitos.Dtos.Autenticacion;
using SanCarlitos.Models;
using SanCarlitos.Repositories.Interfaces;

namespace SanCarlitos.Services.Autenticacion
{

    public sealed class AutenticacionService : IAutenticacionService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly ITokenService _tokenService;

        public AutenticacionService(
            IUsuarioRepository usuarioRepository,
            IPasswordHasher<Usuario> passwordHasher,
            ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<TokenDto?> LoginAsync(LoginDto dto)
        {
            var nombreUsuario = dto.NombreUsuario.Trim();

            var usuario = await _usuarioRepository
                .ObtenerPorNombreUsuarioAsync(nombreUsuario);

            if (usuario is null)
                return null;

            if (usuario.Eliminado || !usuario.Activo)
                return null;

            var resultado = _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.PasswordHash,
                dto.Password);

            if (resultado == PasswordVerificationResult.Failed)
                return null;

            return _tokenService.GenerarToken(usuario);
        }
    }
}

