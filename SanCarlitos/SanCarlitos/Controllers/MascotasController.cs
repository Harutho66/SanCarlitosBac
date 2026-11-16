
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanCarlitos.Data;
using SanCarlitos.Dtos.Mascota;
using SanCarlitos.Services;
using SanCarlitos.Services.Interfaces;

namespace SanCarlitos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    //[Authorize]
    public class MascotasController : ControllerBase
    {
        private readonly IMascotaService _service;
        private readonly VeterinariaContext _context;

        public MascotasController(
            IMascotaService service,
            VeterinariaContext context)
        {
            _service = service;
            _context = context;
        }

        // GET: api/mascotas
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var mascotas = await _service.ObtenerTodosAsync();
                return Ok(mascotas);
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener las mascotas."
                });
            }
        }

        // GET: api/mascotas/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var mascota = await _service.ObtenerPorIdAsync(id);

                if (mascota == null)
                {
                    return NotFound(new
                    {
                        mensaje = "Mascota no encontrada."
                    });
                }

                return Ok(mascota);
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener la mascota."
                });
            }
        }

        // GET: api/mascotas/propietario/5
        [HttpGet("propietario/{propietarioId:int}")]
        public async Task<IActionResult> ObtenerPorPropietario(
            int propietarioId)
        {
            try
            {
                var mascotas = await _service
                    .ObtenerPorPropietarioAsync(propietarioId);

                return Ok(mascotas);
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener las mascotas del propietario."
                });
            }
        }

        // POST: api/mascotas
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearMascotaDto dto)
        {
            try
            {
                var mascota = await _service.CrearAsync(dto);

                return CreatedAtAction(
                    nameof(ObtenerPorId),
                    new { id = mascota.Id },
                    mascota);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new
                {
                    mensaje = "No se pudo guardar la mascota."
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al crear la mascota."
                });
            }
        }

        [Authorize(Roles = "Administrador")]
        // PUT: api/mascotas/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ActualizarMascotaDto dto)
        {
            try
            {
                var actualizado =
                    await _service.ActualizarAsync(id, dto);

                if (!actualizado)
                {
                    return NotFound(new
                    {
                        mensaje = "Mascota no encontrada."
                    });
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new
                {
                    mensaje = "No se pudo actualizar la mascota."
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al actualizar la mascota."
                });
            }
        }

        [Authorize(Roles = "Administrador")]
        // DELETE: api/mascotas/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var eliminado = await _service.EliminarAsync(id);

                if (!eliminado)
                {
                    return NotFound(new
                    {
                        mensaje = "Mascota no encontrada."
                    });
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensaje = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al eliminar la mascota."
                });
            }
        }

        // RF-01
        // GET: api/mascotas/5/historial
        [HttpGet("{id:int}/historial")]
        public async Task<IActionResult> ObtenerHistorial(int id)
        {
            try
            {
                var mascota = await _context.Mascota
                    .AsNoTracking()
                    .Where(m => m.Id == id && !m.Eliminado)
                    .Select(m => new
                    {
                        Mascota = m,
                        Propietario = m.Propietario,

                        Consultas = m.Consulta
                            .Where(c => !c.Eliminado)
                            .OrderByDescending(c => c.FechaConsulta)
                            .Select(c => new
                            {
                                c.Id,
                                c.FechaConsulta,
                                c.Motivo,
                                c.Diagnostico,
                                c.Costo,

                                Veterinario = c.Veterinario,

                                Tratamientos = c.Tratamientos
                                    .Where(t => !t.Eliminado)
                                    .Select(t => new
                                    {
                                        t.Id,
                                        t.Descripcion,
                                        t.Medicamento,
                                        t.Dosis,
                                        t.DuracionDias,
                                        t.Costo
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync();

                if (mascota == null)
                {
                    return NotFound(new
                    {
                        mensaje = "La mascota no existe o está eliminada."
                    });
                }

                if (mascota.Propietario == null ||
                    mascota.Propietario.Eliminado)
                {
                    return NotFound(new
                    {
                        mensaje = "El propietario de la mascota no está disponible."
                    });
                }

                var consultas = mascota.Consultas.Select(c =>
                {
                    var tratamientos = c.Tratamientos
                        .Select(t => new TratamientoHistorialDto
                        {
                            TratamientoId = t.Id,
                            Descripcion = t.Descripcion,
                            Medicamento = t.Medicamento,
                            Dosis = t.Dosis,
                            DuracionDias = t.DuracionDias,
                            Costo = t.Costo
                        })
                        .ToList();

                    decimal costoTratamientos =
                        tratamientos.Sum(t => t.Costo);

                    bool veterinarioActivo =
                        c.Veterinario != null &&
                        !c.Veterinario.Eliminado;

                    return new ConsultaHistorialDto
                    {
                        ConsultaId = c.Id,
                        FechaConsulta = c.FechaConsulta,
                        Motivo = c.Motivo,
                        Diagnostico = c.Diagnostico,

                        VeterinarioId = veterinarioActivo
                            ? c.Veterinario!.Id
                            : 0,

                        VeterinarioNombre = veterinarioActivo
                            ? $"{c.Veterinario!.Nombre} {c.Veterinario.Apellido}"
                            : "Veterinario no disponible",

                        EspecialidadVeterinario = veterinarioActivo
                            ? c.Veterinario!.Especialidad ?? string.Empty
                            : string.Empty,

                        CostoConsulta = c.Costo,
                        Tratamientos = tratamientos,
                        CostoTratamientos = costoTratamientos,
                        CostoTotalConsulta =
                            c.Costo + costoTratamientos
                    };
                }).ToList();

                var resultado = new HistorialClinicoDto
                {
                    MascotaId = mascota.Mascota.Id,
                    NombreMascota = mascota.Mascota.Nombre,
                    Especie = mascota.Mascota.Especie,
                    Raza = mascota.Mascota.Raza,
                    Sexo = mascota.Mascota.Sexo,

                    // Mascota.FechaNacimiento y el DTO usan DateOnly?
                    FechaNacimiento = mascota.Mascota.FechaNacimiento,

                    Propietario = new PropietarioHistorialDto
                    {
                        Id = mascota.Propietario.Id,
                        NombreCompleto =
                            $"{mascota.Propietario.Nombre} {mascota.Propietario.Apellido}",
                        Celular = mascota.Propietario.Celular ?? string.Empty,
                        Direccion = mascota.Propietario.Direccion,
                        Correo = mascota.Propietario.Correo
                    },

                    Consultas = consultas,
                    TotalConsultas = consultas.Count,
                    TotalTratamientos =
                        consultas.Sum(c => c.Tratamientos.Count),
                    CostoTotalAcumulado =
                        consultas.Sum(c => c.CostoTotalConsulta)
                };

                return Ok(resultado);
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener el historial clínico."
                });
            }
        }
        [HttpGet("paginado")]
        public async Task<IActionResult> ObtenerPaginado(
    [FromQuery] MascotaQueryDto query)
        {
            try
            {
                var resultado =
                    await _service.ObtenerPaginadoAsync(query);

                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

    }
}
