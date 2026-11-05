namespace SanCarlitos.Dtos.AtencionesDelDia
{
    public class AtencionesDelDiaDto
    {
        public DateTime Fecha { get; set; }
        public int? VeterinarioId { get; set; }
        public int CantidadConsultas { get; set; }
        public decimal ImporteTotal { get; set; }

        public List<AtencionDetalleDto> Atenciones { get; set; } = new();
    }
}
