using System.ComponentModel.DataAnnotations;

namespace SanCarlitos.Dtos
{
    public class CrearConsultaCompletaDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Debe seleccionar una mascota válida.")]
        public int MascotaId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Debe seleccionar un veterinario válido.")]
        public int VeterinarioId { get; set; }

        [Required(ErrorMessage = "La fecha de consulta es obligatoria.")]
        public DateTime FechaConsulta { get; set; }

        [Required(ErrorMessage = "El motivo es obligatorio.")]
        [StringLength(250, MinimumLength = 3,
            ErrorMessage = "El motivo debe tener entre 3 y 250 caracteres.")]
        public string Motivo { get; set; } = string.Empty;

        [Range(0, 1000000,
            ErrorMessage = "El costo de la consulta no puede ser negativo.")]
        public decimal Costo { get; set; }

        [Required(ErrorMessage = "Debe registrar al menos un tratamiento.")]
        [MinLength(1, ErrorMessage = "Debe registrar al menos un tratamiento.")]
        public List<CrearTratamientoCompletoDto> Tratamientos { get; set; } = new();
    }

    public class CrearTratamientoCompletoDto
    {
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(300, MinimumLength = 1,
            ErrorMessage = "La descripción debe tener entre 1 y 300 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El medicamento es obligatorio.")]
        [StringLength(150, MinimumLength = 1,
            ErrorMessage = "El medicamento debe tener entre 1 y 150 caracteres.")]
        public string Medicamento { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dosis es obligatoria.")]
        [StringLength(100, MinimumLength = 1,
            ErrorMessage = "La dosis debe tener entre 1 y 100 caracteres.")]
        public string Dosis { get; set; } = string.Empty;

        [Range(1, 3650,
            ErrorMessage = "La duración debe estar entre 1 y 3650 días.")]
        public int DuracionDias { get; set; }

        [Range(0, 1000000,
            ErrorMessage = "El costo del tratamiento no puede ser negativo.")]
        public decimal Costo { get; set; }
    }
}