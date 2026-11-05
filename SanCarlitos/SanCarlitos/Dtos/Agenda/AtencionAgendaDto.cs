
namespace SanCarlitos.Dtos.Agenda
{
    public class AtencionAgendaDto
    {
        public string Hora { get; set; } = string.Empty;

        public string Mascota { get; set; } = string.Empty;

        public string Especie { get; set; } = string.Empty;

        public string Propietario { get; set; } = string.Empty;

        public string CelularPropietario { get; set; } = string.Empty;

        public string Motivo { get; set; } = string.Empty;

        public string? Diagnostico { get; set; }

        public decimal Costo { get; set; }
    }
}
