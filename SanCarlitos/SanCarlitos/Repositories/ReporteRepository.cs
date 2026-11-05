
using Microsoft.EntityFrameworkCore;
using SanCarlitos.Data;
using SanCarlitos.Dtos.Reportes;
using SanCarlitos.Dtos.Agenda;
using SanCarlitos.Repositories.Interfaces;

namespace SanCarlitos.Repositories
{
    public class ReporteRepository : IReporteRepository
    {
        private readonly VeterinariaContext _context;

        public ReporteRepository(VeterinariaContext context)
        {
            _context = context;
        }

        // RF-04: Reporte de ingresos por veterinario
        public async Task<List<IngresoVeterinarioDto>>
            ObtenerIngresosVeterinariosAsync(
                DateTime inicio,
                DateTime fin)
        {
            DateTime desde = inicio.Date;
            DateTime hasta = fin.Date.AddDays(1);

            var consultas = await _context.Consulta
                .AsNoTracking()
                .Where(c =>
                    !c.Eliminado &&
                    c.FechaConsulta >= desde &&
                    c.FechaConsulta < hasta)
                .Select(c => new
                {
                    c.VeterinarioId,
                    VeterinarioNombre =
                        c.Veterinario.Nombre + " " +
                        c.Veterinario.Apellido,
                    c.Costo,

                    IngresosTratamientos = c.Tratamientos
                        .Where(t => !t.Eliminado)
                        .Sum(t => (decimal?)t.Costo) ?? 0m
                })
                .ToListAsync();

            return consultas
                .GroupBy(c => new
                {
                    c.VeterinarioId,
                    c.VeterinarioNombre
                })
                .Select(g => new IngresoVeterinarioDto
                {
                    VeterinarioId = g.Key.VeterinarioId,
                    VeterinarioNombre = g.Key.VeterinarioNombre,
                    CantidadConsultas = g.Count(),
                    IngresosConsultas = g.Sum(c => c.Costo),
                    IngresosTratamientos =
                        g.Sum(c => c.IngresosTratamientos),
                    IngresoTotal =
                        g.Sum(c => c.Costo) +
                        g.Sum(c => c.IngresosTratamientos),
                    PromedioPorConsulta = decimal.Round(
                        (g.Sum(c => c.Costo) +
                         g.Sum(c => c.IngresosTratamientos)) / g.Count(),
                        2)
                })
                .OrderByDescending(x => x.IngresoTotal)
                .ToList();
        }

        // RF-12: Agenda diaria de atenciones
        public async Task<List<VeterinarioAgendaDto>>
            ObtenerAgendaDiariaAsync(DateTime fecha)
        {
            DateTime desde = fecha.Date;
            DateTime hasta = desde.AddDays(1);

            // 1. Obtener todos los veterinarios activos.
            var veterinarios = await _context.Veterinarios
                .AsNoTracking()
                .Where(v => !v.Eliminado)
                .OrderBy(v => v.Apellido)
                .ThenBy(v => v.Nombre)
                .Select(v => new VeterinarioAgendaDto
                {
                    VeterinarioId = v.Id,
                    Veterinario = v.Nombre + " " + v.Apellido,
                    Especialidad = v.Especialidad,
                    CantidadConsultas = 0,
                    ImporteTotal = 0m,
                    Atenciones = new List<AtencionAgendaDto>()
                })
                .ToListAsync();

            // 2. Obtener las consultas válidas de todo el día.
            var consultas = await _context.Consulta
                .AsNoTracking()
                .Where(c =>
                    !c.Eliminado &&
                    c.FechaConsulta >= desde &&
                    c.FechaConsulta < hasta &&
                    !c.Veterinario.Eliminado &&
                    !c.Mascota.Eliminado &&
                    !c.Mascota.Propietario.Eliminado)
                .OrderBy(c => c.FechaConsulta)
                .Select(c => new
                {
                    c.VeterinarioId,
                    c.FechaConsulta,
                    Mascota = c.Mascota.Nombre,
                    Especie = c.Mascota.Especie,
                    Propietario =
                        c.Mascota.Propietario.Nombre + " " +
                        c.Mascota.Propietario.Apellido,
                    CelularPropietario =
                        c.Mascota.Propietario.Celular,
                    c.Motivo,
                    c.Diagnostico,
                    c.Costo
                })
                .ToListAsync();

            // 3. Asociar las consultas a cada veterinario.
            foreach (var veterinario in veterinarios)
            {
                var atenciones = consultas
                    .Where(c =>
                        c.VeterinarioId == veterinario.VeterinarioId)
                    .OrderBy(c => c.FechaConsulta)
                    .Select(c => new AtencionAgendaDto
                    {
                        Hora = c.FechaConsulta.ToString("HH:mm"),
                        Mascota = c.Mascota,
                        Especie = c.Especie,
                        Propietario = c.Propietario,
                        CelularPropietario = c.CelularPropietario,
                        Motivo = c.Motivo,
                        Diagnostico = c.Diagnostico,
                        Costo = c.Costo
                    })
                    .ToList();

                veterinario.Atenciones = atenciones;
                veterinario.CantidadConsultas = atenciones.Count;
                veterinario.ImporteTotal =
                    atenciones.Sum(a => a.Costo);
            }

            return veterinarios;
        }
        public async Task<(List<IngresoVeterinarioDto> Items, int TotalCount)>
    ObtenerIngresosVeterinariosPaginadoAsync(
        DateTime inicio,
        DateTime fin,
        int pageNumber,
        int pageSize)
        {
            var ingresos = await ObtenerIngresosVeterinariosAsync(inicio, fin);

            int totalCount = ingresos.Count;

            var items = ingresos
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (items, totalCount);
        }
    }
}
