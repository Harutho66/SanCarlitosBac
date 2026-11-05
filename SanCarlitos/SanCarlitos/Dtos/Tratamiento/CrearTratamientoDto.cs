
using System.ComponentModel.DataAnnotations;

namespace SanCarlitos.Dtos
{
    public class CrearTratamientoDto
    {
        [Required(ErrorMessage = "La consulta es obligatoria")]
        [Range(1, int.MaxValue)]
        public int ConsultaId { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El medicamento es obligatorio")]
        [StringLength(200)]
        public string Medicamento { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dosis es obligatoria")]
        [StringLength(100)]
        public string Dosis { get; set; } = string.Empty;

        [Range(1, 3650, ErrorMessage = "La duración debe ser de al menos 1 día")]
        public int DuracionDias { get; set; }

        [Range(0, 1000000, ErrorMessage = "El costo no puede ser negativo")]
        public decimal Costo { get; set; }
    }
}
