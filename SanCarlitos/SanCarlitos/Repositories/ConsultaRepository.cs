
using Microsoft.EntityFrameworkCore;
using SanCarlitos.Data;
using SanCarlitos.Models;
using SanCarlitos.Repositories.Interfaces;

namespace SanCarlitos.Repositories
{
    public class ConsultaRepository : GenericRepository<Consulta>, IConsultaRepository
    {
        private readonly VeterinariaContext _context;

        public ConsultaRepository(VeterinariaContext context)
     : base(context)
        {
            _context = context;
        }

        public async Task<List<Consulta>> ObtenerTodasConRelacionesAsync()
        {
            return await _context.Consulta
                .AsNoTracking()
                .Include(c => c.Mascota)
                .Include(c => c.Veterinario)
                .Where(c => !c.Eliminado)
                .ToListAsync();
        }

        public async Task<Consulta?> ObtenerConRelacionesPorIdAsync(int id)
        {
            return await _context.Consulta
                .Include(c => c.Mascota)
                .Include(c => c.Veterinario)
                .FirstOrDefaultAsync(c => c.Id == id && !c.Eliminado);
        }

        public async Task<List<Consulta>> ObtenerPorMascotaAsync(int mascotaId)
        {
            return await _context.Consulta
                .AsNoTracking()
                .Include(c => c.Mascota)
                .Include(c => c.Veterinario)
                .Where(c => c.MascotaId == mascotaId && !c.Eliminado)
                .ToListAsync();
        }

        public async Task<List<Consulta>> ObtenerPorVeterinarioAsync(int veterinarioId)
        {
            return await _context.Consulta
                .AsNoTracking()
                .Include(c => c.Mascota)
                .Include(c => c.Veterinario)
                .Where(c => c.VeterinarioId == veterinarioId && !c.Eliminado)
                .ToListAsync();
        }

        public async Task<bool> ExisteMascotaAsync(int mascotaId)
        {
            return await _context.Mascota
                .AnyAsync(m => m.Id == mascotaId && !m.Eliminado);
        }

        public async Task<bool> ExisteVeterinarioAsync(int veterinarioId)
        {
            return await _context.Veterinarios
                .AnyAsync(v => v.Id == veterinarioId && !v.Eliminado);
        }

        // Validación inteligente de disponibilidad

        public async Task<Consulta?> VerificacionDisponibilidadConsulta(
            Consulta consulta)
        {
            DateTime inicio = consulta.FechaConsulta;
            DateTime fin = inicio.AddMinutes(30);

            return await _context.Consulta
                .AsNoTracking()
                .Where(c =>
                    !c.Eliminado &&
                    c.VeterinarioId == consulta.VeterinarioId &&
                    c.Id != consulta.Id &&
                    c.FechaConsulta < fin &&
                    c.FechaConsulta.AddMinutes(30) > inicio)
                .FirstOrDefaultAsync();
        }

        public async Task AgregarAsync(Consulta consulta)
        {
            await _context.Consulta.AddAsync(consulta);
        }

        public void Actualizar(Consulta consulta)
        {
            _context.Consulta.Update(consulta);
        }

        public void Eliminar(Consulta consulta)
        {
            consulta.Eliminado = true;
        }

        public async Task<bool> GuardarCambiosAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}