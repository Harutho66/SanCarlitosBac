using SanCarlitos.Dtos.Autenticacion;
using SanCarlitos.Models;

namespace SanCarlitos.Services.Autenticacion
{
    public interface ITokenService
    {
        TokenDto GenerarToken(Usuario usuario);
    }
}
