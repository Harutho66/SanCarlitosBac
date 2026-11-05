

using SanCarlitos.Models;
using System.Linq.Expressions;

namespace SanCarlitos.Repositories.Interfaces
{
    public interface IConsultaRepository : IGenericRepository<Consulta>
    {
        Task<List<Consulta>> ObtenerTodasConRelacionesAsync();

        Task<Consulta?> ObtenerConRelacionesPorIdAsync(int id);

        Task<List<Consulta>> ObtenerPorMascotaAsync(int mascotaId);

        Task<List<Consulta>> ObtenerPorVeterinarioAsync(int veterinarioId);

        Task<bool> ExisteMascotaAsync(int mascotaId);

        Task<bool> ExisteVeterinarioAsync(int veterinarioId);

        // Verificar disponibilidad del veterinario
        Task<Consulta?> VerificacionDisponibilidadConsulta(
            Consulta consulta);

        Task AgregarAsync(Consulta consulta);

        void Actualizar(Consulta consulta);

        void Eliminar(Consulta consulta);

        Task<bool> GuardarCambiosAsync();

        

    }
}