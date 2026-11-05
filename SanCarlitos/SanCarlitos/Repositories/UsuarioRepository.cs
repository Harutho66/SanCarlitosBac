using Microsoft.EntityFrameworkCore;
using SanCarlitos.Data;
using SanCarlitos.Models;
using SanCarlitos.Repositories.Interfaces;

namespace SanCarlitos.Repositories
{
    public class UsuarioRepository: GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(VeterinariaContext context)
            : base(context)
        {
        }

        public async Task<bool> ExisteNombreUsuarioAsync(
            string nombreUsuario,
            int? excluirId = null)
        {
            string nombreNormalizado = nombreUsuario.Trim();

            var consulta = _context.Usuarios
                .AsNoTracking()
                .Where(u =>
                    !u.Eliminado &&
                    u.NombreUsuario == nombreNormalizado);

            if (excluirId.HasValue)
            {
                consulta = consulta.Where(
                    u => u.Id != excluirId.Value);
            }

            return await consulta.AnyAsync();
        }

        public async Task<bool> ExisteCorreoAsync(
            string correo,
            int? excluirId = null)
        {
            string correoNormalizado = correo.Trim();

            var consulta = _context.Usuarios
                .AsNoTracking()
                .Where(u =>
                    !u.Eliminado &&
                    u.Correo == correoNormalizado);

            if (excluirId.HasValue)
            {
                consulta = consulta.Where(
                    u => u.Id != excluirId.Value);
            }

            return await consulta.AnyAsync();
        }

        public async Task<Usuario?> ObtenerPorNombreUsuarioAsync(
            string nombreUsuario)
        {
            string nombreNormalizado = nombreUsuario.Trim();

            return await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    !u.Eliminado &&
                    u.NombreUsuario == nombreNormalizado);
        }

        public async Task<Usuario?> ObtenerPorCorreoAsync(
            string correo)
        {
            string correoNormalizado = correo.Trim();

            return await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    !u.Eliminado &&
                    u.Correo == correoNormalizado);
        }
    }

}
