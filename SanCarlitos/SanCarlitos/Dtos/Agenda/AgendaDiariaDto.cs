
namespace SanCarlitos.Dtos.Agenda
{
    public class AgendaDiariaDto
    {
        public DateTime Fecha { get; set; }

        public int CantidadVeterinarios { get; set; }

        public int TotalConsultas { get; set; }

        public decimal ImporteTotalConsultas { get; set; }

        public List<VeterinarioAgendaDto> Veterinarios { get; set; }
            = new();
    }
}
