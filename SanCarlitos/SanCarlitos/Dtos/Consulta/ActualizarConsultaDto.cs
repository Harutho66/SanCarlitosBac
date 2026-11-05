using System.ComponentModel.DataAnnotations;

namespace SanCarlitos.Dtos
{
    public class ActualizarConsultaDto
    {
        [Required(ErrorMessage = "La mascota es obligatoria.")]
        public int MascotaId { get; set; }

        [Required(ErrorMessage = "El veterinario es obligatorio.")]
        public int VeterinarioId { get; set; }

        [Required(ErrorMessage = "La fecha de consulta es obligatoria.")]
        public DateTime FechaConsulta { get; set; }

        [Required(ErrorMessage = "El motivo es obligatorio.")]
        [StringLength(200, ErrorMessage = "El motivo no puede superar los 200 caracteres.")]
        public string Motivo { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "El diagnóstico no puede superar los 500 caracteres.")]
        public string? Diagnostico { get; set; }

        [Range(0, 1000000, ErrorMessage = "El costo debe ser mayor o igual a cero.")]
        public decimal Costo { get; set; }
    }
}