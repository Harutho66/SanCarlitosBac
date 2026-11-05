
namespace SanCarlitos.Dtos
{
    public class TratamientoDto
    {
        public int Id { get; set; }

        public int ConsultaId { get; set; }

        public string? ConsultaMotivo { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public string Medicamento { get; set; } = string.Empty;

        public string Dosis { get; set; } = string.Empty;

        public int DuracionDias { get; set; }

        public decimal Costo { get; set; }
    }
}
