using SanCarlitos.Dtos.Paginacion;

namespace SanCarlitos.Dtos.Usuario
{
    public class UsuarioQueryDto : PaginacionQueryDto
    {
        public string NombreUsuario { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string Rol { get; set; } = string.Empty;
        public string OrdenarPor { get; set; } = "Id";
        public bool Descendente { get; set; } = false;
    }
}
