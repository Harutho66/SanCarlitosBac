using AutoMapper;
using SanCarlitos.Dtos.Mascota;

using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Services.Interfaces;
using SanCarlitos.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using SanCarlitos.Models;


namespace SanCarlitos.Services
{
    public class MascotaService : IMascotaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MascotaService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<MascotaDto>>ObtenerTodosAsync()
        {
            var mascotas = await 
                _unitOfWork.Mascotas.ObtenerTodosConPropietarioAsync();

            return _mapper.Map<List<MascotaDto>>(
                mascotas);
        }

        public async Task<MascotaDto?>ObtenerPorIdAsync(int id)
        {
            var mascota = await _unitOfWork
                .Mascotas.ObtenerConPropietarioAsync(id);

            if (mascota == null)
                return null;

            return _mapper.Map<MascotaDto>(
                mascota);
        }

        public async Task<List<MascotaDto>> ObtenerPorPropietarioAsync(int propietarioId)
        {
            var mascotas = await _unitOfWork
                .Mascotas.ObtenerPorPropietarioAsync(
                    propietarioId);

            return _mapper.Map<List<MascotaDto>>(
                mascotas);
        }

        public async Task<MascotaDto> CrearAsync(CrearMascotaDto dto)
        {
            var existePropietario = await _unitOfWork
                .Propietarios.ExisteAsync(
                    dto.PropietarioId);

            if (!existePropietario)
            {
                throw new ArgumentException(
                    "El propietario indicado no existe.");
            }

            var mascota =
                _mapper.Map<Mascota>(dto);

            await _unitOfWork.Mascotas
                .AgregarAsync(mascota);

            await _unitOfWork.GuardarCambiosAsync();

            var mascotaCreada = await _unitOfWork
                .Mascotas.ObtenerConPropietarioAsync(
                    mascota.Id);

            return _mapper.Map<MascotaDto>(
                mascotaCreada!);
        }

        public async Task<bool> ActualizarAsync(
            int id,
            ActualizarMascotaDto dto)
        {
            var mascota = await _unitOfWork
                .Mascotas.ObtenerPorIdAsync(id);

            if (mascota == null)
                return false;

            var existePropietario = await _unitOfWork
                .Propietarios.ExisteAsync(
                    dto.PropietarioId);

            if (!existePropietario)
            {
                throw new ArgumentException(
                    "El propietario indicado no existe.");
            }

            _mapper.Map(dto, mascota);

            await _unitOfWork.GuardarCambiosAsync();

            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var mascota = await _unitOfWork
                .Mascotas.ObtenerPorIdAsync(id);

            if (mascota == null)
                return false;

            var tieneConsultas = await _unitOfWork
                .Mascotas.TieneConsultasActivasAsync(id);

            if (tieneConsultas)
            {
                throw new InvalidOperationException(
                    "No se puede eliminar la mascota " +
                    "porque tiene consultas activas.");
            }

            _unitOfWork.Mascotas.Eliminar(mascota);

            await _unitOfWork.GuardarCambiosAsync();

            return true;
        }
        private static System.Linq.Expressions.Expression<Func<Mascota, bool>>
    CrearFiltro(MascotaQueryDto query)
        {
            string? nombre = string.IsNullOrWhiteSpace(query.Nombre)
                ? null : query.Nombre.Trim();

            string? especie = string.IsNullOrWhiteSpace(query.Especie)
                ? null : query.Especie.Trim();

            int? propietarioId = query.PropietarioId;

            return m =>
                (nombre == null || m.Nombre.Contains(nombre)) &&
                (especie == null || m.Especie == especie) &&
                (!propietarioId.HasValue ||
                    m.PropietarioId == propietarioId.Value);
        }

        private static Func<IQueryable<Mascota>, IOrderedQueryable<Mascota>>
    CrearOrdenamiento(MascotaQueryDto query)
        {
            return query.OrdenarPor.ToLowerInvariant() switch
            {
                "nombre" => query.Descendente
                    ? q => q.OrderByDescending(m => m.Nombre).ThenBy(m => m.Id)
                    : q => q.OrderBy(m => m.Nombre).ThenBy(m => m.Id),

                "especie" => query.Descendente
                    ? q => q.OrderByDescending(m => m.Especie).ThenBy(m => m.Id)
                    : q => q.OrderBy(m => m.Especie).ThenBy(m => m.Id),

                "id" => query.Descendente
                    ? q => q.OrderByDescending(m => m.Id)
                    : q => q.OrderBy(m => m.Id),

                _ => throw new ArgumentException(
                    "OrdenarPor solo admite Id, Nombre o Especie.")
            };
        }

        public async Task<PagedResultDto<MascotaDto>> ObtenerPaginadoAsync(MascotaQueryDto query)
        {
            var (items, totalCount) =
                await _unitOfWork.Mascotas.ObtenerPaginadoAsync(
                    query.PageNumber,
                    query.PageSize,
                    filtro: CrearFiltro(query),
                    ordenar: CrearOrdenamiento(query),
                    incluir: q => q.Include(m => m.Propietario)
                );

            var itemsDto = _mapper.Map<List<MascotaDto>>(items);

            return new PagedResultDto<MascotaDto>
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


