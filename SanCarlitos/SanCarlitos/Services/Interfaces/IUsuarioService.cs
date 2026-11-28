using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Dtos.Usuario;

namespace SanCarlitos.Services.Interfaces
{
    public interface IUsuarioService
    {
            Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync();

            Task<UsuarioDto?> ObtenerPorIdAsync(int id);

            Task<UsuarioDto> CrearAsync(CrearUsuarioDto dto);

            Task<UsuarioDto?> ActualizarAsync(
                int id,
                ActualizarUsuarioDto dto);

            Task<bool> EliminarAsync(int id);

            Task<bool> CambiarPasswordAsync(
                int id,
                CambiarPasswordDto dto);
            Task<PagedResultDto<UsuarioDto>> ObtenerPaginadoAsync(UsuarioQueryDto query);
    }

    }
