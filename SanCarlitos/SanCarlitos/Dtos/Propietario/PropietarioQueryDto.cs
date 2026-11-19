using SanCarlitos.Dtos.Paginacion;

namespace SanCarlitos.Dtos.Propietario
{
    public class PropietarioQueryDto : PaginacionQueryDto
    {
        public string? Nombre { get; set; } 
        public string? Apellido { get; set; }
        public string? Correo { get; set; }
        public string OrdenarPor { get; set; } = "Id";
        public bool Descendente { get; set; } = false;
    }
}
