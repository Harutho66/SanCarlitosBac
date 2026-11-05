using SanCarlitos.Dtos.Mascota;
using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Dtos.Propietario;
using SanCarlitos.Dtos.Veterinario;

namespace SanCarlitos.Services.Interfaces
{
    public interface IVeterinarioService
    {
        Task<List<VeterinarioDto>> ObtenerTodosAsync();

        Task<VeterinarioDto?> ObtenerPorIdAsync(int id);

        Task<VeterinarioDto> CrearAsync(CrearVeterinarioDto dto);
        Task<bool> ActualizarAsync(int id, ActualizarVeterinarioDto dto);

        Task<bool> EliminarAsync(int id);

        Task<PagedResultDto<VeterinarioDto>> ObtenerPaginadoAsync(
    PaginacionQueryDto query);

    }
}
