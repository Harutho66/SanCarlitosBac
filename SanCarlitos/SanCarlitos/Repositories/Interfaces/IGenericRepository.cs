

using SanCarlitos.Models;
using System.Linq.Expressions;

namespace SanCarlitos.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<List<T>> ObtenerTodosAsync();

        Task<T?> ObtenerPorIdAsync(int id);

        Task<bool> ExisteAsync(int id);

        Task AgregarAsync(T entidad);

        void Actualizar(T entidad);

        void Eliminar(T entidad);
        Task<(IReadOnlyList<T> Items, int TotalCount)>
    ObtenerPaginadoAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filtro = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? ordenar = null,
        Func<IQueryable<T>, IQueryable<T>>? incluir = null);

    }
}
