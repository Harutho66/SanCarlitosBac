using Microsoft.EntityFrameworkCore;
using SanCarlitos.Data;
using SanCarlitos.Models;
using SanCarlitos.Repositories.Interfaces;

namespace SanCarlitos.Repositories
{
    public class MascotaRepository : GenericRepository<Mascota>, IMascotaRepository
    {

        public MascotaRepository(VeterinariaContext context) : base(context)
        {
        }


        public async Task<List<Mascota>>
        ObtenerTodosConPropietarioAsync()
        {
            return await _context.Mascota
                .AsNoTracking()
                .Include(m => m.Propietario)
                .Where(m =>
                    !m.Eliminado &&
                    !m.Propietario.Eliminado)
                .ToListAsync();
        }

        public async Task<Mascota?>
            ObtenerConPropietarioAsync(int id)
        {
            return await _context.Mascota
                .AsNoTracking()
                .Include(m => m.Propietario)
                .FirstOrDefaultAsync(m =>
                    m.Id == id &&
                    !m.Eliminado &&
                    !m.Propietario.Eliminado);
        }

        public async Task<List<Mascota>>
            ObtenerPorPropietarioAsync(int propietarioId)
        {
            return await _context.Mascota
                .AsNoTracking()
                .Include(m => m.Propietario)
                .Where(m =>
                    m.PropietarioId == propietarioId &&
                    !m.Eliminado &&
                    !m.Propietario.Eliminado)
                .ToListAsync();
        }

        public async Task<bool> TieneConsultasActivasAsync(int mascotaId)
        {
            return await _context.Consulta
                .AnyAsync(c =>
                    c.MascotaId == mascotaId &&
                    !c.Eliminado);
        }
    }
}
