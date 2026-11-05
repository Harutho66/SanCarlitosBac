using Microsoft.EntityFrameworkCore;
using SanCarlitos.Dtos;

using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Models;
using SanCarlitos.Repositories.Interfaces;
using SanCarlitos.Services.Interfaces;

namespace SanCarlitos.Services
{
    public class TratamientoService : ITratamientoService
    {
        private readonly ITratamientoRepository _repository;

        public TratamientoService(ITratamientoRepository repository)
        {
            _repository = repository;
        }

        private static TratamientoDto ConvertirDto(Tratamiento t)
        {
            return new TratamientoDto
            {
                Id = t.Id,
                ConsultaId = t.ConsultaId,
                ConsultaMotivo = t.Consulta?.Motivo,
                Descripcion = t.Descripcion,
                Medicamento = t.Medicamento ?? string.Empty,
                Dosis = t.Dosis ?? string.Empty,
                DuracionDias = t.DuracionDias,
                Costo = t.Costo
            };
        }

        public async Task<List<TratamientoDto>> ObtenerTodasAsync()
        {
            var tratamientos =
                await _repository.ObtenerTodosConConsultaAsync();

            return tratamientos.Select(ConvertirDto).ToList();
        }

        public async Task<TratamientoDto?> ObtenerPorIdAsync(int id)
        {
            var tratamiento =
                await _repository.ObtenerConConsultaPorIdAsync(id);

            if (tratamiento == null)
                return null;

            return ConvertirDto(tratamiento);
        }

        public async Task<List<TratamientoDto>> ObtenerPorConsultaAsync(
            int consultaId)
        {
            var tratamientos =
                await _repository.ObtenerPorConsultaAsync(consultaId);

            return tratamientos.Select(ConvertirDto).ToList();
        }

        public async Task<(bool Exito, string Mensaje)> CrearAsync(
            CrearTratamientoDto dto)
        {
            if (!await _repository.ExisteConsultaAsync(dto.ConsultaId))
                return (false, "La consulta no existe o está eliminada.");

            var tratamiento = new Tratamiento
            {
                ConsultaId = dto.ConsultaId,
                Descripcion = dto.Descripcion,
                Medicamento = dto.Medicamento,
                Dosis = dto.Dosis,
                DuracionDias = dto.DuracionDias,
                Costo = dto.Costo,
                Eliminado = false
            };

            await _repository.AgregarAsync(tratamiento);

            bool guardado = await _repository.GuardarCambiosAsync();

            return guardado
                ? (true, "Tratamiento creado correctamente.")
                : (false, "No se pudo guardar el tratamiento.");
        }

        public async Task<(bool Exito, string Mensaje)> ActualizarAsync(
            int id, ActualizarTratamientoDto dto)
        {
            var tratamiento =
                await _repository.ObtenerConConsultaPorIdAsync(id);

            if (tratamiento == null)
                return (false, "Tratamiento no encontrado.");

            if (!await _repository.ExisteConsultaAsync(dto.ConsultaId))
                return (false, "La consulta no existe o está eliminada.");

            tratamiento.ConsultaId = dto.ConsultaId;
            tratamiento.Descripcion = dto.Descripcion;
            tratamiento.Medicamento = dto.Medicamento;
            tratamiento.Dosis = dto.Dosis;
            tratamiento.DuracionDias = dto.DuracionDias;
            tratamiento.Costo = dto.Costo;

            _repository.Actualizar(tratamiento);

            bool guardado = await _repository.GuardarCambiosAsync();

            return guardado
                ? (true, "Tratamiento actualizado correctamente.")
                : (false, "No se pudo actualizar el tratamiento.");
        }

        public async Task<(bool Exito, string Mensaje)> EliminarAsync(int id)
        {
            var tratamiento =
                await _repository.ObtenerConConsultaPorIdAsync(id);

            if (tratamiento == null)
                return (false, "Tratamiento no encontrado.");

            _repository.Eliminar(tratamiento);

            bool guardado = await _repository.GuardarCambiosAsync();

            return guardado
                ? (true, "Tratamiento eliminado correctamente.")
                : (false, "No se pudo eliminar el tratamiento.");
        }
    public async Task<PagedResultDto<TratamientoDto>> ObtenerPaginadoAsync(
    PaginacionQueryDto query)
        {
            var (items, totalCount) =
                await _repository.ObtenerPaginadoAsync(
                    query.PageNumber,
                    query.PageSize,
                    ordenar: q => q.OrderBy(t => t.Id),
                    incluir: q => q.Include(t => t.Consulta)
                );

            var itemsDto = items.Select(ConvertirDto).ToList();

            return new PagedResultDto<TratamientoDto>
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