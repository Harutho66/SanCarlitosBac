using SanCarlitos.Dtos.Autenticacion;

namespace SanCarlitos.Services.Autenticacion
{
    public interface IAutenticacionService
    {
        Task<TokenDto?> LoginAsync(LoginDto dto);
    }

}
