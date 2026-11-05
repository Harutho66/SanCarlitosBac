namespace SanCarlitos.Dtos.Reportes
{
    public class IngresosTotalesVeterinariosDto
    {
        public List<IngresoVeterinarioDto> Veterinarios { get; set; } = new();

        public TotalesVeterinariaDto TotalesGenerales { get; set; } = new();
    }
}
