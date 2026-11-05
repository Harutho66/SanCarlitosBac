namespace SanCarlitos.Dtos.Veterinario
{
    public class IngresoVeterinarioDto
    {
        public int VeterinarioId { get; set; }

        public string NombreVeterinario { get; set; } = string.Empty;

        public int CantidadConsultas { get; set; }

        public decimal IngresosConsultas { get; set; }

        public decimal IngresosTratamientos { get; set; }

        public decimal IngresosTotales { get; set; }

        public decimal CostoPromedioAtencion { get; set; }
    }
}
