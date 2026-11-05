using System.ComponentModel.DataAnnotations;

namespace SanCarlitos.Dtos.Usuario
{
    public class CrearUsuarioDto
    {

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Correo { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

    }
}
