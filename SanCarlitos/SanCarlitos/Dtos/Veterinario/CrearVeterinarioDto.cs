using System.ComponentModel.DataAnnotations;

namespace SanCarlitos.Dtos.Veterinario
{
    public class CrearVeterinarioDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especialidad es obligatorio")]
        [StringLength(100)]
        public string Especialidad { get; set; } = string.Empty;

        
        [StringLength(20)]
        public string? Celular { get; set; }

        [Required(ErrorMessage = "La matricula profesional es obligatorio")]
        [StringLength(50)]
        public string MatriculaProfesional { get; set; } = string.Empty;
    }
}
