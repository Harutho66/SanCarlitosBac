using AutoMapper;

using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Dtos.Veterinario;
using SanCarlitos.Models;
using SanCarlitos.Services.Interfaces;
using SanCarlitos.UnitOfWork.Interfaces;

namespace SanCarlitos.Services
{
    public class VeterinarioService: IVeterinarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public VeterinarioService(IUnitOfWork unitOfWork,
            IMapper mapper)
        { 
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarVeterinarioDto dto)
        {
            var veterinario = await _unitOfWork.Veterinarios.ObtenerPorIdAsync(id);
            if (veterinario == null)
                 return false;
            _mapper.Map(dto, veterinario);
            await _unitOfWork.GuardarCambiosAsync();
            return true;
        }

        public async Task<VeterinarioDto> CrearAsync(CrearVeterinarioDto dto)
        {
            var veterinario = _mapper.Map<Veterinario>(dto);
            await _unitOfWork.Veterinarios.AgregarAsync(veterinario);
            await _unitOfWork.GuardarCambiosAsync();
            return _mapper.Map<VeterinarioDto>(veterinario);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var veterinario = await _unitOfWork.Veterinarios.ObtenerPorIdAsync(id);
            if (veterinario == null)
                return false;

            _unitOfWork.Veterinarios.Eliminar(veterinario);
            await _unitOfWork.GuardarCambiosAsync();
            return true;
        }
        

        public async Task<VeterinarioDto?> ObtenerPorIdAsync(int id)
        {
            var veterinario = await _unitOfWork.Veterinarios.ObtenerPorIdAsync(id);
            if (veterinario == null)
                return null;
            return _mapper.Map<VeterinarioDto>(veterinario);

            
        }

        public async Task<List<VeterinarioDto>> ObtenerTodosAsync()
        {
            var veterinario = await _unitOfWork.Veterinarios.ObtenerTodosAsync();
            return _mapper.Map<List<VeterinarioDto>>(veterinario);
        }
        public async Task<PagedResultDto<VeterinarioDto>> ObtenerPaginadoAsync(
    PaginacionQueryDto query)
        {
            var (items, totalCount) =
                await _unitOfWork.Veterinarios.ObtenerPaginadoAsync(
                    query.PageNumber,
                    query.PageSize,
                    ordenar: q => q.OrderBy(v => v.Id)
                );

            var itemsDto = _mapper.Map<List<VeterinarioDto>>(items);

            return new PagedResultDto<VeterinarioDto>
            {
                Items = itemsDto,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(
                    totalCount / (double)query.PageSize)
            };
        }
    }
}
