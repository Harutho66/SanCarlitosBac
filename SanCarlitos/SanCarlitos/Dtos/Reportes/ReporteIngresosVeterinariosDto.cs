namespace SanCarlitos.Dtos.Reportes
{
    public class ReporteIngresosVeterinariosDto
    {
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }

        public List<IngresoVeterinarioDto> Veterinarios { get; set; }
            = new();

        public IngresoVeterinarioDto ResumenGeneral { get; set; }
            = new();
    }
}