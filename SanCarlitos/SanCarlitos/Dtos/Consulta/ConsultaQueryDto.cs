using SanCarlitos.Dtos.Paginacion;

namespace SanCarlitos.Dtos.Consulta
{
    public class ConsultaQueryDto : PaginacionQueryDto
    {
        public string MascotaNombre { get; set; } = string.Empty;
        public string VeterinarioNombre { get; set; } = string.Empty;
        public string? Diagnostico { get; set; }
        public string OrdenarPor { get; set; } = "Id";
        public bool Descendente { get; set; } = false;
    }
}
