namespace SanCarlitos.Dtos.AtencionesDelDia
{
    public class AtencionDetalleDto
    {
        public int ConsultaId { get; set; }
        public string Hora { get; set; } = string.Empty;

        public MascotaAtencionDto Mascota { get; set; } = new();
        public PropietarioAtencionDto Propietario { get; set; } = new();
        public VeterinarioAtencionDto Veterinario { get; set; } = new();

        public string Motivo { get; set; } = string.Empty;
        public string? Diagnostico { get; set; }
        public decimal CostoConsulta { get; set; }

        public List<TratamientoAtencionDto> Tratamientos { get; set; } = new();

        public decimal CostoTotalAtencion { get; set; }
    }
}
