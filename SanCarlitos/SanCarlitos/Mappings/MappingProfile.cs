

using AutoMapper;
using SanCarlitos.Dtos;
using SanCarlitos.Dtos.Mascota;
using SanCarlitos.Dtos.Propietario;
using SanCarlitos.Dtos.Usuario;
using SanCarlitos.Dtos.Veterinario;
using SanCarlitos.Models;

namespace SanCarlitos.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ==================================
            // PROPIETARIO
            // ==================================

            CreateMap<CrearPropietarioDto, Propietario>();
            CreateMap<ActualizarPropietarioDto, Propietario>();
            CreateMap<Propietario, PropietarioDto>();

            // ==================================
            // MASCOTA
            // ==================================

            CreateMap<CrearMascotaDto, Mascota>();
            CreateMap<ActualizarMascotaDto, Mascota>();

            CreateMap<Mascota, MascotaDto>()
                .ForMember(
                    destino => destino.Propietario,
                    opciones => opciones.MapFrom(
                        origen =>
                            origen.Propietario.Nombre + " " +
                            origen.Propietario.Apellido));

            // ==================================
            // VETERINARIO
            // ==================================

            CreateMap<CrearVeterinarioDto, Veterinario>();
            CreateMap<ActualizarVeterinarioDto, Veterinario>();
            CreateMap<Veterinario, VeterinarioDto>();

            // ==================================
            // CONSULTA
            // ==================================

            CreateMap<Consulta, ConsultaDto>()
                .ForMember(
                    destino => destino.MascotaNombre,
                    opciones => opciones.MapFrom(
                        origen => origen.Mascota.Nombre))
                .ForMember(
                    destino => destino.VeterinarioNombre,
                    opciones => opciones.MapFrom(
                        origen =>
                            origen.Veterinario.Nombre + " " +
                            origen.Veterinario.Apellido));

            CreateMap<CrearConsultaDto, Consulta>();
            CreateMap<ActualizarConsultaDto, Consulta>();

            // ==================================
            // TRATAMIENTO
            // ==================================

            CreateMap<Tratamiento, TratamientoDto>()
                .ForMember(
                    destino => destino.ConsultaMotivo,
                    opciones => opciones.MapFrom(
                        origen => origen.Consulta.Motivo));

            CreateMap<CrearTratamientoDto, Tratamiento>();
            CreateMap<ActualizarTratamientoDto, Tratamiento>();
            CreateMap<Usuario, UsuarioDto>();

            CreateMap<CrearUsuarioDto, Usuario>()
                .ForMember(
                    destino => destino.PasswordHash,
                    opciones => opciones.Ignore())
                .ForMember(
                    destino => destino.Rol,
                    opciones => opciones.Ignore());

            CreateMap<ActualizarUsuarioDto, Usuario>()
                .ForMember(
                    destino => destino.PasswordHash,
                    opciones => opciones.Ignore())
                .ForMember(
                    destino => destino.Rol,
                    opciones => opciones.Ignore())
                .ForMember(
                    destino => destino.Eliminado,
                    opciones => opciones.Ignore())
                .ForMember(
                    destino => destino.FechaRegistro,
                    opciones => opciones.Ignore());
        }
    }

}
    

