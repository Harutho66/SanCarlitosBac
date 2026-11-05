using System.ComponentModel.DataAnnotations;

namespace SanCarlitos.Dtos.Propietario
{
    public class ActualizarPropietarioDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Celular { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Direccion { get; set; }

        [EmailAddress]
        [StringLength(150)]
        public string? Correo { get; set; }








    }

}
