using SanCarlitos.Dtos.Paginacion;

namespace SanCarlitos.Dtos.Reportes
{
    public class ReporteQueryDto : PaginacionQueryDto
    {
        public string VeterinarioNombre { get; set; } = string.Empty;
        public int CantidadConsultas { get; set; }
        public string OrdenarPor { get; set; } = "Id";
        public bool Descendente { get; set; } = false;
    }
}
