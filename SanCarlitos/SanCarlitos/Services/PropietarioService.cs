using AutoMapper;

using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Dtos.Propietario;
using SanCarlitos.Models;
using SanCarlitos.Services.Interfaces;
using SanCarlitos.UnitOfWork.Interfaces;

namespace SanCarlitos.Services
{
    public class PropietarioService : IPropietarioService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PropietarioService(IMapper mapper,
                            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }

        public async Task<List<PropietarioDto>> ObtenerTodosAsync()
        {
            var propietarios = await _unitOfWork
                .Propietarios.ObtenerTodosAsync();

            return _mapper.Map<List<PropietarioDto>>(
                propietarios);
        }
        public async Task<PropietarioDto?>
            ObtenerPorIdAsync(int id)
        {
            var propietario = await _unitOfWork
                .Propietarios.ObtenerPorIdAsync(id);

            if (propietario == null)
                return null;

            return _mapper.Map<PropietarioDto>(
                propietario);
        }

        public async Task<PropietarioDto> CrearAsync(
            CrearPropietarioDto dto)
        {
            var propietario =
                _mapper.Map<Propietario>(dto);

            await _unitOfWork.Propietarios
                .AgregarAsync(propietario);

            await _unitOfWork.GuardarCambiosAsync();

            return _mapper.Map<PropietarioDto>(
                propietario);
        }

        public async Task<bool> ActualizarAsync(
            int id,
            ActualizarPropietarioDto dto)
        {
            var propietario = await _unitOfWork
                .Propietarios.ObtenerPorIdAsync(id);

            if (propietario == null)
                return false;

            _mapper.Map(dto, propietario);

            await _unitOfWork.GuardarCambiosAsync();

            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var propietario = await _unitOfWork
                .Propietarios.ObtenerPorIdAsync(id);

            if (propietario == null)
                return false;

            var mascotas = await _unitOfWork
                .Mascotas.ObtenerPorPropietarioAsync(id);

            if (mascotas.Count > 0)
            {
                throw new InvalidOperationException(
                    "No se puede eliminar el propietario " +
                    "porque tiene mascotas activas.");
            }

            _unitOfWork.Propietarios.Eliminar(
                propietario);

            await _unitOfWork.GuardarCambiosAsync();

            return true;
        }

        public async Task<PagedResultDto<PropietarioDto>>
            ObtenerPaginadoAsync(PropietarioQueryDto query)
        {
            string? nombre = string.IsNullOrWhiteSpace(query.Nombre)
                ? null : query.Nombre.Trim();

            string? apellido = string.IsNullOrWhiteSpace(query.Apellido)
                ? null : query.Apellido.Trim();

            string? correo = string.IsNullOrWhiteSpace(query.Correo)
                ? null : query.Correo.Trim();

            var (items, totalCount) =
                await _unitOfWork.Propietarios.ObtenerPaginadoAsync(
                    query.PageNumber,
                    query.PageSize,
                    p =>
                        (nombre == null && apellido == null && correo == null)
                        || (nombre != null && p.Nombre.Contains(nombre))
                        || (apellido != null && p.Apellido.Contains(apellido))
                        || (correo != null && p.Correo != null &&
                            p.Correo.Contains(correo))
                );

            var itemsDto = _mapper.Map<List<PropietarioDto>>(items);

            return new PagedResultDto<PropietarioDto>
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

