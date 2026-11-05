namespace SanCarlitos.Dtos.Autenticacion
{
    public sealed class TokenDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public string TokenType { get; set; } = "Bearer";

        public DateTime ExpiresAtUtc { get; set; }

        public int UsuarioId { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        public string Rol { get; set; } = string.Empty;
    }

}
