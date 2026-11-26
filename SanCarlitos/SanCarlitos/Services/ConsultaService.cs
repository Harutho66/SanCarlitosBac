using Microsoft.EntityFrameworkCore;
using SanCarlitos.Data;
using SanCarlitos.Dtos;

using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Models;
using SanCarlitos.Repositories.Interfaces;
using SanCarlitos.Services.Interfaces;
using System.Data;

namespace SanCarlitos.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _repository;
        private readonly VeterinariaContext _context;

        public ConsultaService(
            IConsultaRepository repository,
            VeterinariaContext context)
        {
            _repository = repository;
            _context = context;
        }

        private static ConsultaDto ConvertirDto(Consulta c)
        {
            return new ConsultaDto
            {
                Id = c.Id,
                MascotaId = c.MascotaId,
                MascotaNombre = c.Mascota?.Nombre ?? string.Empty,
                VeterinarioId = c.VeterinarioId,
                VeterinarioNombre = c.Veterinario == null
                    ? string.Empty
                    : $"{c.Veterinario.Nombre} {c.Veterinario.Apellido}".Trim(),
                FechaConsulta = c.FechaConsulta,
                Motivo = c.Motivo,
                Diagnostico = c.Diagnostico,
                Costo = c.Costo
            };
        }

        // Obtener todas las consultas.
        public async Task<List<ConsultaDto>> ObtenerTodasAsync()
        {
            var consultas =
                await _repository.ObtenerTodasConRelacionesAsync();

            return consultas.Select(ConvertirDto).ToList();
        }

        // Obtener una consulta por su ID.
        public async Task<ConsultaDto?> ObtenerPorIdAsync(int id)
        {
            var consulta =
                await _repository.ObtenerConRelacionesPorIdAsync(id);

            return consulta == null ? null : ConvertirDto(consulta);
        }

        // Obtener consultas por mascota.
        public async Task<List<ConsultaDto>> ObtenerPorMascotaAsync(
            int mascotaId)
        {
            var consultas =
                await _repository.ObtenerPorMascotaAsync(mascotaId);

            return consultas.Select(ConvertirDto).ToList();
        }

        // Obtener consultas por veterinario.
        public async Task<List<ConsultaDto>> ObtenerPorVeterinarioAsync(
            int veterinarioId)
        {
            var consultas =
                await _repository.ObtenerPorVeterinarioAsync(veterinarioId);

            return consultas.Select(ConvertirDto).ToList();
        }

        // Crear consulta.
        // Reutiliza ProgramarAsync para validar disponibilidad.
        public async Task<(bool Exito, string Mensaje)> CrearAsync(
            CrearConsultaDto dto)
        {
            return await ProgramarAsync(dto);
        }

        // RF-02: programar consulta validando disponibilidad del veterinario.
        public async Task<(bool Exito, string Mensaje)> ProgramarAsync(
            CrearConsultaDto dto)
        {
            if (dto.FechaConsulta <= DateTime.Now)
            {
                return (false,
                    "La fecha de la consulta debe ser futura.");
            }

            if (string.IsNullOrWhiteSpace(dto.Motivo))
            {
                return (false,
                    "El motivo de la consulta es obligatorio.");
            }

            if (dto.Costo < 0)
            {
                return (false,
                    "El costo no puede ser negativo.");
            }

            await using var transaccion =
                await _context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

            try
            {
                if (!await _repository.ExisteMascotaAsync(dto.MascotaId))
                {
                    await transaccion.RollbackAsync();

                    return (false,
                        "La mascota no existe o está eliminada.");
                }

                if (!await _repository.ExisteVeterinarioAsync(
                    dto.VeterinarioId))
                {
                    await transaccion.RollbackAsync();

                    return (false,
                        "El veterinario no existe o está eliminado.");
                }

                var consulta = new Consulta
                {
                    MascotaId = dto.MascotaId,
                    VeterinarioId = dto.VeterinarioId,
                    FechaConsulta = dto.FechaConsulta,
                    Motivo = dto.Motivo.Trim(),
                    Diagnostico = dto.Diagnostico,
                    Costo = dto.Costo,
                    Eliminado = false
                };

                var consultaEncontrada =
                    await _repository.VerificacionDisponibilidadConsulta(
                        consulta);

                if (consultaEncontrada != null)
                {
                    await transaccion.RollbackAsync();

                    return (false,
                        "409: El veterinario ya tiene una consulta " +
                        "programada en ese horario. Seleccione otro horario.");
                }

                await _repository.AgregarAsync(consulta);

                bool guardado =
                    await _repository.GuardarCambiosAsync();

                if (!guardado)
                {
                    await transaccion.RollbackAsync();

                    return (false,
                        "No se pudo guardar la consulta.");
                }

                await transaccion.CommitAsync();

                return (true,
                    "Consulta programada correctamente.");
            }
            catch
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }

        // Actualizar una consulta.
        public async Task<(bool Exito, string Mensaje)> ActualizarAsync(
            int id,
            ActualizarConsultaDto dto)
        {
            var consulta =
                await _repository.ObtenerConRelacionesPorIdAsync(id);

            if (consulta == null)
            {
                return (false, "Consulta no encontrada.");
            }

            if (dto.FechaConsulta <= DateTime.Now)
            {
                return (false,
                    "La fecha de la consulta debe ser futura.");
            }

            if (string.IsNullOrWhiteSpace(dto.Motivo))
            {
                return (false,
                    "El motivo de la consulta es obligatorio.");
            }

            if (dto.Costo < 0)
            {
                return (false,
                    "El costo no puede ser negativo.");
            }

            if (!await _repository.ExisteMascotaAsync(dto.MascotaId))
            {
                return (false,
                    "La mascota no existe o está eliminada.");
            }

            if (!await _repository.ExisteVeterinarioAsync(
                dto.VeterinarioId))
            {
                return (false,
                    "El veterinario no existe o está eliminado.");
            }

            consulta.MascotaId = dto.MascotaId;
            consulta.VeterinarioId = dto.VeterinarioId;
            consulta.FechaConsulta = dto.FechaConsulta;
            consulta.Motivo = dto.Motivo.Trim();
            consulta.Diagnostico = dto.Diagnostico;
            consulta.Costo = dto.Costo;

            _repository.Actualizar(consulta);

            bool guardado =
                await _repository.GuardarCambiosAsync();

            return guardado
                ? (true, "Consulta actualizada correctamente.")
                : (false, "No se pudo actualizar la consulta.");
        }

        // Eliminación lógica de una consulta.
        public async Task<(bool Exito, string Mensaje)> EliminarAsync(
            int id)
        {
            var consulta =
                await _repository.ObtenerConRelacionesPorIdAsync(id);

            if (consulta == null)
            {
                return (false, "Consulta no encontrada.");
            }

            _repository.Eliminar(consulta);

            bool guardado =
                await _repository.GuardarCambiosAsync();

            return guardado
                ? (true, "Consulta eliminada correctamente.")
                : (false, "No se pudo eliminar la consulta.");
        }

        // RF-03: registrar consulta y varios tratamientos
        // en una sola transacción.
        public async Task<(
            bool Exito,
            string Mensaje,
            int? ConsultaId,
            decimal? CostoTotal)> CrearCompletaAsync(
                CrearConsultaCompletaDto dto)
        {
            if (dto.FechaConsulta <= DateTime.Now)
            {
                return (
                    false,
                    "La fecha de consulta debe ser futura.",
                    null,
                    null);
            }

            if (string.IsNullOrWhiteSpace(dto.Motivo) ||
                dto.Motivo.Trim().Length < 3 ||
                dto.Motivo.Trim().Length > 250)
            {
                return (
                    false,
                    "El motivo debe tener entre 3 y 250 caracteres.",
                    null,
                    null);
            }

            if (dto.Costo < 0 || dto.Costo > 1000000)
            {
                return (
                    false,
                    "El costo de la consulta no es válido.",
                    null,
                    null);
            }

            if (dto.Tratamientos == null ||
                dto.Tratamientos.Count == 0)
            {
                return (
                    false,
                    "Debe registrar al menos un tratamiento.",
                    null,
                    null);
            }

            foreach (var t in dto.Tratamientos)
            {
                if (string.IsNullOrWhiteSpace(t.Descripcion) ||
                    t.Descripcion.Trim().Length > 300 ||
                    string.IsNullOrWhiteSpace(t.Medicamento) ||
                    t.Medicamento.Trim().Length > 150 ||
                    string.IsNullOrWhiteSpace(t.Dosis) ||
                    t.Dosis.Trim().Length > 100 ||
                    t.DuracionDias < 1 ||
                    t.DuracionDias > 3650 ||
                    t.Costo < 0 ||
                    t.Costo > 1000000)
                {
                    return (
                        false,
                        "Uno o más tratamientos contienen datos inválidos.",
                        null,
                        null);
                }
            }

            await using var transaccion =
                await _context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

            try
            {
                if (!await _repository.ExisteMascotaAsync(dto.MascotaId))
                {
                    await transaccion.RollbackAsync();

                    return (
                        false,
                        "La mascota no existe o está eliminada.",
                        null,
                        null);
                }

                if (!await _repository.ExisteVeterinarioAsync(
                    dto.VeterinarioId))
                {
                    await transaccion.RollbackAsync();

                    return (
                        false,
                        "El veterinario no existe o está eliminado.",
                        null,
                        null);
                }

                var consulta = new Consulta
                {
                    MascotaId = dto.MascotaId,
                    VeterinarioId = dto.VeterinarioId,
                    FechaConsulta = dto.FechaConsulta,
                    Motivo = dto.Motivo.Trim(),
                    Diagnostico = dto.Diagnostico.Trim(),
                    Costo = dto.Costo,
                    Eliminado = false
                };

                var conflicto =
                    await _repository.VerificacionDisponibilidadConsulta(
                        consulta);

                if (conflicto != null)
                {
                    await transaccion.RollbackAsync();

                    return (
                        false,
                        "409: El veterinario ya tiene una consulta " +
                        "programada en ese horario. Seleccione otro horario.",
                        null,
                        null);
                }

                await _repository.AgregarAsync(consulta);

                // Guardar la consulta para obtener su ID.
                await _context.SaveChangesAsync();

                var tratamientos = dto.Tratamientos.Select(t =>
                    new Tratamiento
                    {
                        ConsultaId = consulta.Id,
                        Descripcion = t.Descripcion.Trim(),
                        Medicamento = t.Medicamento.Trim(),
                        Dosis = t.Dosis.Trim(),
                        DuracionDias = t.DuracionDias,
                        Costo = t.Costo,
                        Eliminado = false
                    }).ToList();

                await _context.Tratamientos.AddRangeAsync(tratamientos);

                // Guardar los tratamientos en la misma transacción.
                await _context.SaveChangesAsync();

                decimal costoTotal =
                    consulta.Costo + tratamientos.Sum(t => t.Costo);

                await transaccion.CommitAsync();

                return (
                    true,
                    "Consulta y tratamientos registrados correctamente.",
                    consulta.Id,
                    costoTotal);
            }
            catch
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }
        public async Task<PagedResultDto<ConsultaDto>> ObtenerPaginadoAsync(
    PaginacionQueryDto query)
        {
            var (items, totalCount) =
                await _repository.ObtenerPaginadoAsync(
                    query.PageNumber,
                    query.PageSize,
                    ordenar: q => q.OrderBy(c => c.Id),
                    incluir: q => q
                        .Include(c => c.Mascota)
                        .Include(c => c.Veterinario)
                );

            var itemsDto = items.Select(ConvertirDto).ToList();

            return new PagedResultDto<ConsultaDto>
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