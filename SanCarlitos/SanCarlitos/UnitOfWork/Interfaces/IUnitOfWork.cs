
using SanCarlitos.Repositories.Interfaces;

namespace SanCarlitos.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IPropietarioRepository Propietarios{ get; }

        IMascotaRepository Mascotas{ get; }
        IVeterinarioRepository Veterinarios { get; }
        IConsultaRepository Consultas { get; }
        ITratamientoRepository Tratamientos { get; }
        IUsuarioRepository Usuarios { get; }
        IReporteRepository Reportes { get; }
        Task<int> GuardarCambiosAsync();


    }
}
