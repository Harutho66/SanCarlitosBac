

using SanCarlitos.Models;

namespace SanCarlitos.Repositories.Interfaces
{
    public interface ITratamientoRepository :
        IGenericRepository<Tratamiento>
    {
        Task<List<Tratamiento>> ObtenerTodosConConsultaAsync();

        Task<Tratamiento?> ObtenerConConsultaPorIdAsync(int id);

        Task<List<Tratamiento>> ObtenerPorConsultaAsync(int consultaId);

        Task<bool> ExisteConsultaAsync(int consultaId);

        Task<bool> GuardarCambiosAsync();
    }
}