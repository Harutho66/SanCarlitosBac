namespace SanCarlitos.Dtos.Mascota
{
    public class MascotaDto
    {
        public int Id { get; set; }

        public int PropietarioId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Especie { get; set; } = string.Empty;

        public string? Raza { get; set; }

        public string Sexo { get; set; } = string.Empty;

        public DateOnly? FechaNacimiento { get; set; }

        public string Propietario { get; set; } = string.Empty;

        public DateTime FechaRegistro { get; set; }
    }
}

