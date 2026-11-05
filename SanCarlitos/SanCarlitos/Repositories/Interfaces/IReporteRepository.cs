
using SanCarlitos.Dtos.Reportes;
using SanCarlitos.Dtos.Agenda;

namespace SanCarlitos.Repositories.Interfaces
{
    public interface IReporteRepository
    {
        // RF-04: Reporte de ingresos por veterinario
        Task<List<IngresoVeterinarioDto>>
            ObtenerIngresosVeterinariosAsync(
                DateTime inicio,
                DateTime fin);
        Task<(List<IngresoVeterinarioDto> Items, int TotalCount)>
    ObtenerIngresosVeterinariosPaginadoAsync(
        DateTime inicio,
        DateTime fin,
        int pageNumber,
        int pageSize);
        // RF-12: Agenda diaria de atenciones
        Task<List<VeterinarioAgendaDto>>
            ObtenerAgendaDiariaAsync(DateTime fecha);
    }
}
