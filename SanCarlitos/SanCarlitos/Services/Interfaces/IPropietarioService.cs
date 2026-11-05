using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Dtos.Propietario;

namespace SanCarlitos.Services.Interfaces
{
    public interface IPropietarioService
    {

        Task<List<PropietarioDto>> ObtenerTodosAsync();

        Task<PropietarioDto?> ObtenerPorIdAsync(int id);

        Task<PropietarioDto> CrearAsync(CrearPropietarioDto dto);

        Task<bool> ActualizarAsync(int id,ActualizarPropietarioDto dto);

        Task<bool> EliminarAsync(int id);
        Task<PagedResultDto<PropietarioDto>>
    ObtenerPaginadoAsync(PaginacionQueryDto query);

    }
}
