using SanCarlitos.Dtos.Paginacion;

namespace SanCarlitos.Dtos.Veterinario
{
    public class VeterinarioQueryDto : PaginacionQueryDto
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Especialidad { get; set; }
        public string OrdenarPor { get; set; } = "Id";
        public bool Descendente { get; set; } = false;
    }
}
