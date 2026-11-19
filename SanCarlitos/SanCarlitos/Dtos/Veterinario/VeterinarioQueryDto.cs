using SanCarlitos.Dtos.Paginacion;

namespace SanCarlitos.Dtos.Veterinario
{
    public class VeterinarioQueryDto : PaginacionQueryDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public string OrdenarPor { get; set; } = "Id";
        public bool Descendente { get; set; } = false;
    }
}
