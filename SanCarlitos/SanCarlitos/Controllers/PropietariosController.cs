using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Dtos.Propietario;
using SanCarlitos.Services;
using SanCarlitos.Services.Interfaces;

namespace SanCarlitos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PropietariosController : ControllerBase
    {
        private readonly IPropietarioService _service;

        public PropietariosController(IPropietarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var propietarios = await _service.ObtenerTodosAsync();

            return Ok(propietarios);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var propietario =
                await _service.ObtenerPorIdAsync(id);

            if (propietario == null)
                return NotFound("Propietario no encontrado.");

            return Ok(propietario);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(
            CrearPropietarioDto dto)
        {
            var propietario =
                await _service.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = propietario.Id },
                propietario);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(
            int id,
            ActualizarPropietarioDto dto)
        {
            var actualizado =
                await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound("Propietario no encontrado.");

            return NoContent();
        }


        //[Authorize(Roles = "Administrador")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var eliminado =
                    await _service.EliminarAsync(id);

                if (!eliminado)
                    return NotFound("Propietario no encontrado.");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
        [HttpGet("paginado")]
        public async Task<IActionResult> ObtenerPaginado(
    [FromQuery] PaginacionQueryDto query)
        {
            var resultado =
                await _service.ObtenerPaginadoAsync(query);

            return Ok(resultado);
        }

    }
}
