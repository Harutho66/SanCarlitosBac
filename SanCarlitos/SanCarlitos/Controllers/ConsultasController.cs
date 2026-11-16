using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanCarlitos.Dtos;
using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Services.Interfaces;

namespace SanCarlitos.Controllers
{
    [ApiController]
    [Route("api/consultas")]

    //[Authorize]
    public class ConsultasController : ControllerBase
    {
        private readonly IConsultaService _service;

        public ConsultasController(IConsultaService service)
        {
            _service = service;
        }

        // GET: api/consultas
        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            try
            {
                var consultas = await _service.ObtenerTodasAsync();
                return Ok(consultas);
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener las consultas."
                });
            }
        }

        // GET: api/consultas/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var consulta = await _service.ObtenerPorIdAsync(id);

                if (consulta == null)
                {
                    return NotFound(new
                    {
                        mensaje = "Consulta no encontrada."
                    });
                }

                return Ok(consulta);
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener la consulta."
                });
            }
        }

        // GET: api/consultas/mascota/5
        [HttpGet("mascota/{mascotaId:int}")]
        public async Task<IActionResult> ObtenerPorMascota(int mascotaId)
        {
            try
            {
                var consultas =
                    await _service.ObtenerPorMascotaAsync(mascotaId);

                return Ok(consultas);
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener las consultas de la mascota."
                });
            }
        }

        // GET: api/consultas/veterinario/5
        [HttpGet("veterinario/{veterinarioId:int}")]
        public async Task<IActionResult> ObtenerPorVeterinario(
            int veterinarioId)
        {
            try
            {
                var consultas =
                    await _service.ObtenerPorVeterinarioAsync(veterinarioId);

                return Ok(consultas);
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener las consultas del veterinario."
                });
            }
        }

        // POST: api/consultas
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearConsultaDto dto)
        {
            try
            {
                var resultado = await _service.CrearAsync(dto);

                if (!resultado.Exito)
                {
                    if (resultado.Mensaje.StartsWith("409:"))
                    {
                        return Conflict(new
                        {
                            mensaje = resultado.Mensaje
                                .Replace("409:", "").Trim()
                        });
                    }

                    if (resultado.Mensaje.Contains("no existe"))
                    {
                        return NotFound(new
                        {
                            mensaje = resultado.Mensaje
                        });
                    }

                    return BadRequest(new
                    {
                        mensaje = resultado.Mensaje
                    });
                }

                return Ok(new
                {
                    mensaje = resultado.Mensaje
                });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new
                {
                    mensaje = "No se pudo guardar la consulta."
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al crear la consulta."
                });
            }
        }

        // RF-02
        // POST: api/consultas/programar
        [HttpPost("programar")]
        public async Task<IActionResult> Programar(
            [FromBody] CrearConsultaDto dto)
        {
            try
            {
                var resultado = await _service.ProgramarAsync(dto);

                if (!resultado.Exito)
                {
                    if (resultado.Mensaje.StartsWith("409:"))
                    {
                        return Conflict(new
                        {
                            mensaje = resultado.Mensaje
                                .Replace("409:", "").Trim()
                        });
                    }

                    if (resultado.Mensaje.Contains("no existe"))
                    {
                        return NotFound(new
                        {
                            mensaje = resultado.Mensaje
                        });
                    }

                    return BadRequest(new
                    {
                        mensaje = resultado.Mensaje
                    });
                }

                return Ok(new
                {
                    mensaje = resultado.Mensaje
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    mensaje = ex.Message
                });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new
                {
                    mensaje = "No se pudo guardar la consulta."
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error inesperado al programar la consulta."
                });
            }
        }

        // RF-03
        // POST: api/consultas/completa
        // Registra una consulta y sus tratamientos en una sola operación.
        [HttpPost("completa")]
        public async Task<IActionResult> CrearCompleta(
            [FromBody] CrearConsultaCompletaDto dto)
        {
            try
            {
                var resultado = await _service.CrearCompletaAsync(dto);

                if (!resultado.Exito)
                {
                    if (resultado.Mensaje.StartsWith("409:"))
                    {
                        return Conflict(new
                        {
                            mensaje = resultado.Mensaje
                                .Replace("409:", "").Trim()
                        });
                    }

                    if (resultado.Mensaje.Contains("no existe"))
                    {
                        return NotFound(new
                        {
                            mensaje = resultado.Mensaje
                        });
                    }

                    return BadRequest(new
                    {
                        mensaje = resultado.Mensaje
                    });
                }

                return Ok(new
                {
                    mensaje = resultado.Mensaje,
                    consultaId = resultado.ConsultaId,
                    costoTotal = resultado.CostoTotal
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    mensaje = ex.Message
                });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new
                {
                    mensaje = "No se pudo registrar la consulta y sus tratamientos."
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error inesperado al registrar la consulta completa."
                });
            }
        }

        // PUT: api/consultas/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ActualizarConsultaDto dto)
        {
            try
            {
                var resultado = await _service.ActualizarAsync(id, dto);

                if (!resultado.Exito)
                {
                    if (resultado.Mensaje.Contains("no encontrada"))
                    {
                        return NotFound(new
                        {
                            mensaje = resultado.Mensaje
                        });
                    }

                    if (resultado.Mensaje.Contains("no existe"))
                    {
                        return NotFound(new
                        {
                            mensaje = resultado.Mensaje
                        });
                    }

                    return BadRequest(new
                    {
                        mensaje = resultado.Mensaje
                    });
                }

                return Ok(new
                {
                    mensaje = resultado.Mensaje
                });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new
                {
                    mensaje = "No se pudo actualizar la consulta."
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al actualizar la consulta."
                });
            }
        }

        [Authorize(Roles = "Administrador")]
        // DELETE: api/consultas/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var resultado = await _service.EliminarAsync(id);

                if (!resultado.Exito)
                {
                    return NotFound(new
                    {
                        mensaje = resultado.Mensaje
                    });
                }

                return Ok(new
                {
                    mensaje = resultado.Mensaje
                });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new
                {
                    mensaje = "No se pudo eliminar la consulta."
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al eliminar la consulta."
                });
            }
        }
        // GET: api/consultas/paginado
        [HttpGet("paginado")]
        public async Task<IActionResult> ObtenerPaginado(
            [FromQuery] PaginacionQueryDto query)
        {
            try
            {
                var resultado = await _service.ObtenerPaginadoAsync(query);
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
                    mensaje = "Error al obtener las consultas paginadas."
                });
            }
        }
    }
}