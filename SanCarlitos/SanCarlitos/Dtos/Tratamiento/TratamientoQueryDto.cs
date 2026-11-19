using SanCarlitos.Dtos.Paginacion;

namespace SanCarlitos.Dtos.Tratamiento
{
    public class TratamientoQueryDto : PaginacionQueryDto
    {
        public int ConsultaId { get; set; }
        public string Medicamento { get; set; } = string.Empty;
        public string Dosis { get; set; } = string.Empty;
        public string OrdenarPor { get; set; } = "Id";
        public bool Descendente { get; set; } = false;
    }
}
