using System.ComponentModel.DataAnnotations;

namespace SanCarlitos.Dtos.Usuario
{
    public class CambiarPasswordDto
    {
        [Required]
        public string PasswordActual { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string PasswordNueva { get; set; } = string.Empty;

    }
}
