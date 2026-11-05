using SanCarlitos.Data;
using SanCarlitos.Models;
using SanCarlitos.Repositories.Interfaces;

namespace SanCarlitos.Repositories
{
    public class PropietarioRepository : GenericRepository<Propietario>, IPropietarioRepository
    {
        public PropietarioRepository(VeterinariaContext context) : base(context)
        {
        }

    }
}
