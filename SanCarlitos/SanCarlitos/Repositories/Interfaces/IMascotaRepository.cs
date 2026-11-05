

using SanCarlitos.Models;

namespace SanCarlitos.Repositories.Interfaces
{
    public interface IMascotaRepository : IGenericRepository<Mascota>
    {

        Task<List<Mascota>>ObtenerTodosConPropietarioAsync();

        Task<Mascota?>ObtenerConPropietarioAsync(int id);

        Task<List<Mascota>>ObtenerPorPropietarioAsync(int propietarioId);

        Task<bool> TieneConsultasActivasAsync(int mascotaId);

    }
}
