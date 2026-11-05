using SanCarlitos.Dtos;
using SanCarlitos.Dtos.Paginacion;

namespace SanCarlitos.Services.Interfaces
{
    public interface ITratamientoService
    {
        Task<List<TratamientoDto>> ObtenerTodasAsync();

        Task<TratamientoDto?> ObtenerPorIdAsync(int id);

        Task<List<TratamientoDto>> ObtenerPorConsultaAsync(int consultaId);

        Task<(bool Exito, string Mensaje)> CrearAsync(
            CrearTratamientoDto dto);

        Task<(bool Exito, string Mensaje)> ActualizarAsync(
            int id,
            ActualizarTratamientoDto dto);

        Task<(bool Exito, string Mensaje)> EliminarAsync(int id);
        Task<PagedResultDto<TratamientoDto>> ObtenerPaginadoAsync(
    PaginacionQueryDto query);
    }
}