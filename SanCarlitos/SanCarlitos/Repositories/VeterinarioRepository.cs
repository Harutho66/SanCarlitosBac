using SanCarlitos.Data;
using SanCarlitos.Models;
using SanCarlitos.Repositories.Interfaces;

namespace SanCarlitos.Repositories
{
    public class VeterinarioRepository:GenericRepository<Veterinario>,IVeterinarioRepository
    {
        public VeterinarioRepository(VeterinariaContext context) : base(context) { }
    }
}
