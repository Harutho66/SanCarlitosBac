
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SanCarlitos.Dtos;
using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Dtos.Tratamiento;
using SanCarlitos.Services.Interfaces;

namespace SanCarlitos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    //[Authorize]
    public class TratamientosController : ControllerBase
    {
        private readonly ITratamientoService _service;

        public TratamientosController(ITratamientoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var tratamientos = await _service.ObtenerTodasAsync();
            return Ok(tratamientos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var tratamiento = await _service.ObtenerPorIdAsync(id);

            if (tratamiento == null)
                return NotFound("Tratamiento no encontrado.");

            return Ok(tratamiento);
        }

        [HttpGet("consulta/{consultaId:int}")]
        public async Task<IActionResult> ObtenerPorConsulta(int consultaId)
        {
            var tratamientos =
                await _service.ObtenerPorConsultaAsync(consultaId);

            return Ok(tratamientos);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Crear(CrearTratamientoDto dto)
        {
            var resultado = await _service.CrearAsync(dto);

            if (!resultado.Exito)
                return BadRequest(resultado.Mensaje);

            return Ok(resultado.Mensaje);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(
            int id, ActualizarTratamientoDto dto)
        {
            var resultado = await _service.ActualizarAsync(id, dto);

            if (!resultado.Exito)
            {
                if (resultado.Mensaje == "Tratamiento no encontrado.")
                    return NotFound(resultado.Mensaje);

                return BadRequest(resultado.Mensaje);
            }

            return Ok(resultado.Mensaje);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _service.EliminarAsync(id);

            if (!resultado.Exito)
            {
                if (resultado.Mensaje == "Tratamiento no encontrado.")
                    return NotFound(resultado.Mensaje);

                return BadRequest(resultado.Mensaje);
            }

            return Ok(resultado.Mensaje);
        }
        [HttpGet("paginado")]
        public async Task<IActionResult> ObtenerPaginado(
    [FromQuery] TratamientoQueryDto query)
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
