
using SanCarlitos.Dtos.Agenda;
using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Dtos.Reportes;

namespace SanCarlitos.Services.Interfaces
{
    public interface IReporteService
    {
        // RF-04: Reporte de ingresos por veterinario
        Task<ReporteIngresosVeterinariosDto>
            ObtenerIngresosVeterinariosAsync(
                DateTime inicio,
                DateTime fin);

        // RF-12: Agenda diaria de atenciones
        Task<AgendaDiariaDto>
            ObtenerAgendaDiariaAsync(DateTime? fecha);
        Task<PagedResultDto<IngresoVeterinarioDto>>
    ObtenerIngresosVeterinariosPaginadoAsync(
        DateTime inicio,
        DateTime fin,
        PaginacionQueryDto query);
    }
}
