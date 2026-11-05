using System.ComponentModel.DataAnnotations;

namespace SanCarlitos.Dtos.Usuario
{
    public class ActualizarUsuarioDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Correo { get; set; } = string.Empty;

        public bool Activo { get; set; }

    }
}
