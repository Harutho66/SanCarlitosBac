namespace SanCarlitos.Dtos.Reportes
{
    public class IngresoVeterinarioDto
    {
        public int VeterinarioId { get; set; }
        public string VeterinarioNombre { get; set; } = string.Empty;

        public int CantidadConsultas { get; set; }

        public decimal IngresosConsultas { get; set; }
        public decimal IngresosTratamientos { get; set; }
        public decimal IngresoTotal { get; set; }
        public decimal PromedioPorConsulta { get; set; }
    }
}