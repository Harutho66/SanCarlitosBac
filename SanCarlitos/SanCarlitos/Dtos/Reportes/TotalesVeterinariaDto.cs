namespace SanCarlitos.Dtos.Reportes
{
    public class TotalesVeterinariaDto
    {
        public int CantidadConsultas { get; set; }

        public decimal IngresosConsultas { get; set; }

        public decimal IngresosTratamientos { get; set; }

        public decimal IngresosTotales { get; set; }

        public decimal CostoPromedioAtencion { get; set; }
    }
}
