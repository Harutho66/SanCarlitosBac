
using SanCarlitos.Dtos.Agenda;
using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Dtos.Reportes;
using SanCarlitos.Repositories.Interfaces;
using SanCarlitos.Services.Interfaces;
using SanCarlitos.UnitOfWork.Interfaces;

namespace SanCarlitos.Services
{
    public class ReporteService : IReporteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReporteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // RF-04: Reporte de ingresos por veterinario
        public async Task<ReporteIngresosVeterinariosDto>
            ObtenerIngresosVeterinariosAsync(
                DateTime inicio,
                DateTime fin)
        {
            var ingresos = await _unitOfWork.Reportes
                .ObtenerIngresosVeterinariosAsync(inicio, fin);

            var resumen = new IngresoVeterinarioDto
            {
                CantidadConsultas = ingresos.Sum(
                    x => x.CantidadConsultas),

                IngresosConsultas = ingresos.Sum(
                    x => x.IngresosConsultas),

                IngresosTratamientos = ingresos.Sum(
                    x => x.IngresosTratamientos),

                IngresoTotal = ingresos.Sum(
                    x => x.IngresoTotal)
            };

            return new ReporteIngresosVeterinariosDto
            {
                Inicio = inicio.Date,
                Fin = fin.Date,
                Veterinarios = ingresos,
                ResumenGeneral = resumen
            };
        }

        // RF-12: Agenda diaria de atenciones
        public async Task<AgendaDiariaDto>
            ObtenerAgendaDiariaAsync(DateTime? fecha)
        {
            DateTime fechaAgenda = (fecha ?? DateTime.Today).Date;

            var veterinarios = await _unitOfWork.Reportes
                .ObtenerAgendaDiariaAsync(fechaAgenda);

            return new AgendaDiariaDto
            {
                Fecha = fechaAgenda,

                CantidadVeterinarios = veterinarios.Count,

                TotalConsultas = veterinarios.Sum(
                    v => v.CantidadConsultas),

                ImporteTotalConsultas = veterinarios.Sum(
                    v => v.ImporteTotal),

                Veterinarios = veterinarios
            };
        }
        public async Task<PagedResultDto<IngresoVeterinarioDto>>
    ObtenerIngresosVeterinariosPaginadoAsync(
        DateTime inicio,
        DateTime fin,
        PaginacionQueryDto query)
        {
            var (items, totalCount) = await _unitOfWork.Reportes
                .ObtenerIngresosVeterinariosPaginadoAsync(
                    inicio,
                    fin,
                    query.PageNumber,
                    query.PageSize);

            return new PagedResultDto<IngresoVeterinarioDto>
            {
                Items = items,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(
                    totalCount / (double)query.PageSize)
            };
        }
    }
}
