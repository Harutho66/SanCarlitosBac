
using SanCarlitos.Dtos;
using SanCarlitos.Dtos.Paginacion;

namespace SanCarlitos.Services.Interfaces
{
    public interface IConsultaService
    {
        Task<List<ConsultaDto>> ObtenerTodasAsync();

        Task<ConsultaDto?> ObtenerPorIdAsync(int id);

        Task<List<ConsultaDto>> ObtenerPorMascotaAsync(int mascotaId);

        Task<List<ConsultaDto>> ObtenerPorVeterinarioAsync(int veterinarioId);

        Task<(bool Exito, string Mensaje)> CrearAsync(
            CrearConsultaDto dto);

        // RF-02: programar consulta validando disponibilidad
        Task<(bool Exito, string Mensaje)> ProgramarAsync(
            CrearConsultaDto dto);

        Task<(bool Exito, string Mensaje)> ActualizarAsync(
            int id,
            ActualizarConsultaDto dto);
        Task<(bool Exito, string Mensaje, int? ConsultaId, decimal? CostoTotal)>
            CrearCompletaAsync(CrearConsultaCompletaDto dto);

        Task<(bool Exito, string Mensaje)> EliminarAsync(int id);
        Task<PagedResultDto<ConsultaDto>> ObtenerPaginadoAsync(
    PaginacionQueryDto query);
    }
}
