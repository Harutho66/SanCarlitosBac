using Microsoft.EntityFrameworkCore;
using SanCarlitos.Data;
using SanCarlitos.Models;
using SanCarlitos.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SanCarlitos.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
                where T : BaseEntity
    {

        protected readonly VeterinariaContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(VeterinariaContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }


        public async Task<List<T>> ObtenerTodosAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => !x.Eliminado==true)
                .ToListAsync();
        }

        public async Task<T?> ObtenerPorIdAsync(int id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x =>
                    x.Id == id && !x.Eliminado);
        }
        public async Task<bool> ExisteAsync(int id)
        {
            return await _dbSet
                .AnyAsync(x =>
                    x.Id == id && !x.Eliminado);
        }

        public async Task AgregarAsync(T entidad)
        {
            entidad.Eliminado = false;
            entidad.FechaRegistro = DateTime.UtcNow.AddHours(-4);

            await _dbSet.AddAsync(entidad);
        }

        public void Actualizar(T entidad)
        {
            _dbSet.Update(entidad);
        }

        public void Eliminar(T entidad)
        {
            entidad.Eliminado = true;
        }

        public async Task<(IReadOnlyList<T> Items, int TotalCount)>
            ObtenerPaginadoAsync(
                int pageNumber,
                int pageSize,
                Expression<Func<T, bool>>? filtro = null,
                Func<IQueryable<T>, IOrderedQueryable<T>>? ordenar = null,
                Func<IQueryable<T>, IQueryable<T>>? incluir = null)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber));

            if (pageSize < 1 || pageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            IQueryable<T> consulta = _dbSet
                .AsNoTracking()
                .Where(x => !x.Eliminado);

            if (filtro != null)
                consulta = consulta.Where(filtro);

            int totalCount = await consulta.CountAsync();

            var consultaOrdenada = ordenar != null
                ? ordenar(consulta)
                : consulta.OrderBy(x => x.Id);

            IQueryable<T> consultaFinal = incluir != null
                ? incluir(consultaOrdenada)
                : consultaOrdenada;

            int saltar = (int)((long)(pageNumber - 1) * pageSize);

            var items = await consultaFinal
                .Skip(saltar)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }


    }
}
