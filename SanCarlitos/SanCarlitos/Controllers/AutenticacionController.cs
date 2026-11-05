using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanCarlitos.Dtos.Autenticacion;
using SanCarlitos.Services.Autenticacion;

namespace SanCarlitos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AutenticacionController : ControllerBase
    {
        private readonly IAutenticacionService _autenticacionService;

        public AutenticacionController(
            IAutenticacionService autenticacionService)
        {
            _autenticacionService = autenticacionService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TokenDto>> Login(
            [FromBody] LoginDto dto)
        {
            var token = await _autenticacionService.LoginAsync(dto);

            if (token is null)
            {
                return Unauthorized(new
                {
                    mensaje = "Usuario o contraseña incorrectos."
                });
            }

            return Ok(token);
        }
    }
}
