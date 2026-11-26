using SanCarlitos.Dtos.Paginacion;

namespace SanCarlitos.Dtos.Mascota
{
    public class MascotaQueryDto : PaginacionQueryDto
    {
        public string? Nombre { get; set; }
        public string? Especie { get; set; }
        public string? PropietarioNombre { get; set; }
        public string OrdenarPor { get; set; } = "Id";
        public bool Descendente { get; set; } = false;

    }
}
