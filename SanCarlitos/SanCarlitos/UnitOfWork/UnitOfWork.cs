using SanCarlitos.Data;
using SanCarlitos.Models;
using SanCarlitos.Repositories.Interfaces;
using SanCarlitos.UnitOfWork.Interfaces;

namespace SanCarlitos.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VeterinariaContext _context;

        private readonly IUsuarioRepository _usuarios;
        public IPropietarioRepository Propietarios { get; }
        public IMascotaRepository Mascotas { get; }
        public IVeterinarioRepository Veterinarios { get; }
        public IConsultaRepository Consultas { get; }
        public ITratamientoRepository Tratamientos { get; }
        public IUsuarioRepository Usuarios => _usuarios;
        public IReporteRepository Reportes { get; }

        public UnitOfWork(
            VeterinariaContext context,
            IPropietarioRepository propietarios,
            IMascotaRepository mascotas,
            IVeterinarioRepository veterinarios,
            IConsultaRepository consultas,
            ITratamientoRepository tratamientos,
            IReporteRepository reportes,
            IUsuarioRepository usuarios)
        {
            _context = context;
            Propietarios = propietarios;
            Mascotas = mascotas;
            Veterinarios = veterinarios;
            Consultas = consultas;
            Tratamientos = tratamientos;
            Reportes = reportes;
            _usuarios = usuarios;
        }

        public async Task<int> GuardarCambiosAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}