namespace SanCarlitos.Dtos
{
    public class ConsultaDto
    {
        public int Id { get; set; }

        public int MascotaId { get; set; }

        public string MascotaNombre { get; set; } = string.Empty;

        public int VeterinarioId { get; set; }

        public string VeterinarioNombre { get; set; } = string.Empty;

        public DateTime FechaConsulta { get; set; }

        public string Motivo { get; set; } = string.Empty;

        public string? Diagnostico { get; set; }

        public decimal Costo { get; set; }
    }
}