using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanCarlitos.Dtos.Usuario;
using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Services.Interfaces;

namespace SanCarlitos.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> ObtenerTodos()
        {
            var usuarios = await _usuarioService.ObtenerTodosAsync();

            return Ok(usuarios);
        }

        // GET: api/usuarios/paginado
        [HttpGet("paginado")]
        public async Task<IActionResult> ObtenerPaginado(
            [FromQuery] UsuarioQueryDto query)
        {
            try
            {
                var resultado =
                    await _usuarioService.ObtenerPaginadoAsync(query);

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
                    mensaje = "Error al obtener los usuarios paginados."
                });
            }
        }

        // GET: api/usuarios/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsuarioDto>> ObtenerPorId(int id)
        {
            var usuario = await _usuarioService.ObtenerPorIdAsync(id);

            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            return Ok(usuario);
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> Crear(
            [FromBody] CrearUsuarioDto dto)
        {
            try
            {
                var usuario = await _usuarioService.CrearAsync(dto);

                return CreatedAtAction(
                    nameof(ObtenerPorId),
                    new { id = usuario.Id },
                    usuario);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensaje = ex.Message });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    mensaje = "No se pudo registrar el usuario. Verifica que el nombre de usuario y el correo no estén duplicados."
                });
            }
        }

        // PUT: api/usuarios/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<UsuarioDto>> Actualizar(
            int id,
            [FromBody] ActualizarUsuarioDto dto)
        {
            try
            {
                var usuario = await _usuarioService.ActualizarAsync(id, dto);

                if (usuario == null)
                    return NotFound(new { mensaje = "Usuario no encontrado." });

                return Ok(usuario);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensaje = ex.Message });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    mensaje = "No se pudo actualizar el usuario. Verifica que el nombre de usuario y el correo no estén duplicados."
                });
            }
        }

        // DELETE: api/usuarios/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                bool eliminado = await _usuarioService.EliminarAsync(id);

                if (!eliminado)
                    return NotFound(new { mensaje = "Usuario no encontrado." });

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // PUT: api/usuarios/5/cambiar-password
        [HttpPut("{id:int}/cambiar-password")]
        public async Task<IActionResult> CambiarPassword(
            int id,
            [FromBody] CambiarPasswordDto dto)
        {
            try
            {
                bool cambiado = await _usuarioService.CambiarPasswordAsync(
                    id,
                    dto);

                if (!cambiado)
                    return NotFound(new { mensaje = "Usuario no encontrado." });

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new
                {
                    mensaje = "No se pudo guardar el cambio de contraseña."
                });
            }
        }
    }
}