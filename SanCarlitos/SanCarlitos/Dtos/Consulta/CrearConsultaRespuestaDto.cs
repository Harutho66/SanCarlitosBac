namespace SanCarlitos.Dtos.Consulta
{
    public class CrearConsultaRespuestaDto
    {
        public CrearConsultaDto ConsultaDto { get; set; } = null!;

        public List<CrearTratamientoDto> TratamientosDto { get; set; } = new();

        public decimal CostoTotal { get; set; }
    }
}
