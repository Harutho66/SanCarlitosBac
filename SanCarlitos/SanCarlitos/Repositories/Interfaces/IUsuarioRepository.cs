using SanCarlitos.Models;

namespace SanCarlitos.Repositories.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<bool> ExisteNombreUsuarioAsync(
            string nombreUsuario,
            int? excluirId = null);

        Task<bool> ExisteCorreoAsync(
            string correo,
            int? excluirId = null);

        Task<Usuario?> ObtenerPorNombreUsuarioAsync(
            string nombreUsuario);

        Task<Usuario?> ObtenerPorCorreoAsync(
            string correo);
    }

}
