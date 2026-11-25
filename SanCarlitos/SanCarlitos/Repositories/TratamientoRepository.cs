
using Microsoft.EntityFrameworkCore;
using SanCarlitos.Data;
using SanCarlitos.Models;
using SanCarlitos.Repositories.Interfaces;

namespace SanCarlitos.Repositories
{
    public class TratamientoRepository : GenericRepository<Tratamiento>, ITratamientoRepository
    {
        public TratamientoRepository(VeterinariaContext context)
    : base(context)
        {
        }
        public async Task<List<Tratamiento>> ObtenerTodosConConsultaAsync()
        {
            return await _context.Tratamientos
                .AsNoTracking()
                .Include(t => t.Consulta)
                .Where(t => !t.Eliminado)
                .ToListAsync();
        }

        public async Task<Tratamiento?> ObtenerConConsultaPorIdAsync(int id)
        {
            return await _context.Tratamientos
                .Include(t => t.Consulta)
                .FirstOrDefaultAsync(t => t.Id == id && !t.Eliminado);
        }

        public async Task<List<Tratamiento>> ObtenerPorConsultaAsync(int consultaId)
        {
            return await _context.Tratamientos
                .AsNoTracking()
                .Include(t => t.Consulta)
                .Where(t => t.ConsultaId == consultaId && !t.Eliminado)
                .ToListAsync();
        }

        public async Task<bool> ExisteConsultaAsync(int consultaId)
        {
            return await _context.Consulta
                .AnyAsync(c => c.Id == consultaId && !c.Eliminado);
        }

        public async Task AgregarAsync(Tratamiento tratamiento)
        {
            await _context.Tratamientos.AddAsync(tratamiento);
        }

        public void Actualizar(Tratamiento tratamiento)
        {
            _context.Tratamientos.Update(tratamiento);
        }

        public void Eliminar(Tratamiento tratamiento)
        {
            tratamiento.Eliminado = true;
        }

        public async Task<bool> GuardarCambiosAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
