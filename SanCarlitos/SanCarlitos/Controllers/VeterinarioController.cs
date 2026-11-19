using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Dtos.Veterinario;
using SanCarlitos.Services.Interfaces;

namespace SanCarlitos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    //[Authorize]
    public class VeterinariosController : ControllerBase
    {
        private readonly IVeterinarioService _service;

        public VeterinariosController(
            IVeterinarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<VeterinarioDto>>> ObtenerTodos()
        {
            var veterinarios = await _service.ObtenerTodosAsync();
            return Ok(veterinarios);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VeterinarioDto>> ObtenerPorId(int id)
        {
            var veterinario = await _service.ObtenerPorIdAsync(id);

            if (veterinario == null)
                return NotFound("Veterinario no encontrado.");

            return Ok(veterinario);
        }


        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult<VeterinarioDto>> Crear(
            [FromBody] CrearVeterinarioDto dto)
        {
            var veterinario = await _service.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = veterinario.id },
                veterinario);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ActualizarVeterinarioDto dto)
        {
            var actualizado = await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound("Veterinario no encontrado.");

            return NoContent();
        }

        //[Authorize(Roles = "Administrador")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var eliminado = await _service.EliminarAsync(id);

                if (!eliminado)
                    return NotFound("Veterinario no encontrado.");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensaje = ex.Message });
            }
        }
        [HttpGet("paginado")]
        public async Task<IActionResult> ObtenerPaginado(
    [FromQuery] VeterinarioQueryDto query)
        {
            try
            {
                var resultado = await _service.ObtenerPaginadoAsync(query);

                return Ok(resultado);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}