
namespace SanCarlitos.Dtos.Agenda
{
    public class VeterinarioAgendaDto
    {
        public int VeterinarioId { get; set; }

        public string Veterinario { get; set; } = string.Empty;

        public string Especialidad { get; set; } = string.Empty;

        public int CantidadConsultas { get; set; }

        public decimal ImporteTotal { get; set; }

        public List<AtencionAgendaDto> Atenciones { get; set; } = new();
    }
}
