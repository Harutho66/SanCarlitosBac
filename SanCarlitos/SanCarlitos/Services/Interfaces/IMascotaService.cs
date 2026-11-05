using SanCarlitos.Dtos.Mascota;
using SanCarlitos.Dtos.Paginacion;

namespace SanCarlitos.Services.Interfaces
{
    public interface IMascotaService
    {

        Task<List<MascotaDto>> ObtenerTodosAsync();

        Task<MascotaDto?> ObtenerPorIdAsync(int id);

        Task<List<MascotaDto>>ObtenerPorPropietarioAsync(int propietarioId);

        Task<MascotaDto> CrearAsync(CrearMascotaDto dto);

        Task<bool> ActualizarAsync(int id,ActualizarMascotaDto dto);

        Task<bool> EliminarAsync(int id);

        Task<PagedResultDto<MascotaDto>>
     ObtenerPaginadoAsync(MascotaQueryDto query);


    }
}
