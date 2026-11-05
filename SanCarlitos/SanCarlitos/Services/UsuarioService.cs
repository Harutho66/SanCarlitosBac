using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SanCarlitos.Dtos.Paginacion;
using SanCarlitos.Dtos.Usuario;
using SanCarlitos.Models;
using SanCarlitos.Services.Interfaces;
using SanCarlitos.UnitOfWork.Interfaces;

namespace SanCarlitos.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuarioService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPasswordHasher<Usuario> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync()
        {
            var usuarios = await _unitOfWork.Usuarios.ObtenerTodosAsync();

            return _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
        }

        public async Task<UsuarioDto?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del usuario no es válido.");

            var usuario = await _unitOfWork.Usuarios.ObtenerPorIdAsync(id);

            if (usuario == null || usuario.Eliminado)
                return null;

            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<UsuarioDto> CrearAsync(CrearUsuarioDto dto)
        {
            string nombreUsuario = dto.NombreUsuario.Trim();
            string correo = dto.Correo.Trim();

            if (await _unitOfWork.Usuarios
                .ExisteNombreUsuarioAsync(nombreUsuario))
            {
                throw new InvalidOperationException(
                    "El nombre de usuario ya está registrado.");
            }

            if (await _unitOfWork.Usuarios
                .ExisteCorreoAsync(correo))
            {
                throw new InvalidOperationException(
                    "El correo electrónico ya está registrado.");
            }

            var usuario = _mapper.Map<Usuario>(dto);

            usuario.NombreUsuario = nombreUsuario;
            usuario.Correo = correo;
            usuario.Rol = "Usuario";
            usuario.Activo = true;
            usuario.Eliminado = false;
            usuario.FechaRegistro = DateTime.UtcNow;

            usuario.PasswordHash = _passwordHasher.HashPassword(
                usuario,
                dto.Password);

            await _unitOfWork.Usuarios.AgregarAsync(usuario);
            await _unitOfWork.GuardarCambiosAsync();

            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<UsuarioDto?> ActualizarAsync(
            int id,
            ActualizarUsuarioDto dto)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del usuario no es válido.");

            var usuario = await _unitOfWork.Usuarios.ObtenerPorIdAsync(id);

            if (usuario == null || usuario.Eliminado)
                return null;

            string nombreUsuario = dto.NombreUsuario.Trim();
            string correo = dto.Correo.Trim();

            if (await _unitOfWork.Usuarios
                .ExisteNombreUsuarioAsync(nombreUsuario, id))
            {
                throw new InvalidOperationException(
                    "El nombre de usuario ya está registrado.");
            }

            if (await _unitOfWork.Usuarios
                .ExisteCorreoAsync(correo, id))
            {
                throw new InvalidOperationException(
                    "El correo electrónico ya está registrado.");
            }

            _mapper.Map(dto, usuario);

            usuario.NombreUsuario = nombreUsuario;
            usuario.Correo = correo;

            _unitOfWork.Usuarios.Actualizar(usuario);
            await _unitOfWork.GuardarCambiosAsync();

            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del usuario no es válido.");

            var usuario = await _unitOfWork.Usuarios.ObtenerPorIdAsync(id);

            if (usuario == null || usuario.Eliminado)
                return false;

            _unitOfWork.Usuarios.Eliminar(usuario);

            await _unitOfWork.GuardarCambiosAsync();

            return true;
        }

        public async Task<bool> CambiarPasswordAsync(
            int id,
            CambiarPasswordDto dto)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del usuario no es válido.");

            var usuario = await _unitOfWork.Usuarios.ObtenerPorIdAsync(id);

            if (usuario == null || usuario.Eliminado)
                return false;

            var resultado = _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.PasswordHash,
                dto.PasswordActual);

            if (resultado == PasswordVerificationResult.Failed)
            {
                throw new InvalidOperationException(
                    "La contraseña actual es incorrecta.");
            }

            if (dto.PasswordActual == dto.PasswordNueva)
            {
                throw new InvalidOperationException(
                    "La nueva contraseña debe ser diferente de la actual.");
            }

            usuario.PasswordHash = _passwordHasher.HashPassword(
                usuario,
                dto.PasswordNueva);

            _unitOfWork.Usuarios.Actualizar(usuario);
            await _unitOfWork.GuardarCambiosAsync();

            return true;
        }

        public async Task<PagedResultDto<UsuarioDto>> ObtenerPaginadoAsync(
        PaginacionQueryDto query)
        {
            var (usuarios, totalCount) =
                await _unitOfWork.Usuarios.ObtenerPaginadoAsync(
                    query.PageNumber,
                    query.PageSize,
                    ordenar: u => u.OrderBy(x => x.Id));

            var usuariosDto = _mapper.Map<List<UsuarioDto>>(usuarios);

            return new PagedResultDto<UsuarioDto>
            {
                Items = usuariosDto,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(
                    totalCount / (double)query.PageSize)
            };
        }

    }
}
