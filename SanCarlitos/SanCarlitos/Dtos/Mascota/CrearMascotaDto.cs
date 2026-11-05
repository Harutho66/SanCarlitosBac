using System.ComponentModel.DataAnnotations;

namespace SanCarlitos.Dtos.Mascota
{
    public class CrearMascotaDto
    {

        [Range(1, int.MaxValue)]
        public int PropietarioId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Especie { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Raza { get; set; }

        [Required]
        [RegularExpression("^(Macho|Hembra)$",ErrorMessage = "El sexo debe ser Macho o Hembra.")]
        public string Sexo { get; set; } = string.Empty;

        public DateOnly? FechaNacimiento { get; set; }

    }
}
