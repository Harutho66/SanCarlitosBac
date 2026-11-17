using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Services.Interfaces;

namespace SanCarlitos.Controllers
{
    [ApiController]
    [Route("api/reportes")]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteService _service;

        public ReportesController(IReporteService service)
        {
            _service = service;
        }

        //[Authorize(Roles = "Administrador")]
        [HttpGet("ingresos-veterinarios")]
        public async Task<IActionResult> ObtenerIngresosVeterinarios(
            [FromQuery] DateTime inicio,
            [FromQuery] DateTime fin)
        {
            try
            {
                if (inicio == default || fin == default)
                {
                    return BadRequest(new
                    {
                        mensaje = "Debe proporcionar las fechas de inicio y fin."
                    });
                }

                if (inicio.Date > fin.Date)
                {
                    return BadRequest(new
                    {
                        mensaje = "La fecha inicial no puede ser mayor que la final."
                    });
                }

                var resultado = await _service
                    .ObtenerIngresosVeterinariosAsync(inicio, fin);

                return Ok(resultado);
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al generar el reporte financiero."
                });
            }
        }
        // RF-04: Reporte de ingresos por veterinario paginado
        [HttpGet("ingresos-veterinarios/paginado")]
        public async Task<IActionResult> ObtenerIngresosVeterinariosPaginado(
            [FromQuery] DateTime inicio,
            [FromQuery] DateTime fin,
            [FromQuery] PaginacionQueryDto query)
        {
            try
            {
                if (inicio == default || fin == default)
                {
                    return BadRequest(new
                    {
                        mensaje = "Debe proporcionar las fechas de inicio y fin."
                    });
                }

                if (inicio.Date > fin.Date)
                {
                    return BadRequest(new
                    {
                        mensaje = "La fecha inicial no puede ser mayor que la final."
                    });
                }

                var resultado = await _service
                    .ObtenerIngresosVeterinariosPaginadoAsync(
                        inicio,
                        fin,
                        query);

                return Ok(resultado);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al generar el reporte financiero paginado."
                });
            }
        }

        // RF-12: Agenda diaria de atenciones por veterinario
        [HttpGet("agenda-diaria")]
        public async Task<IActionResult> ObtenerAgendaDiaria(
            [FromQuery] DateTime? fecha = null)
        {
            try
            {
                var resultado = await _service
                    .ObtenerAgendaDiariaAsync(fecha);

                return Ok(resultado);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new
                    {
                        mensaje = "Error al obtener la agenda diaria."
                    });
            }
        }
    }
}