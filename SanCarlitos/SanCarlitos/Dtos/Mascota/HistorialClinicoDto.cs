
namespace SanCarlitos.Dtos.Mascota
{
    public class HistorialClinicoDto
    {
        public int MascotaId { get; set; }
        public string NombreMascota { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string? Raza { get; set; }
        public string Sexo { get; set; } = string.Empty;
        public DateOnly? FechaNacimiento { get; set; }

        public PropietarioHistorialDto Propietario { get; set; }
            = new();

        public List<ConsultaHistorialDto> Consultas { get; set; }
            = new();

        public int TotalConsultas { get; set; }
        public int TotalTratamientos { get; set; }
        public decimal CostoTotalAcumulado { get; set; }
    }

    public class PropietarioHistorialDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public string? Direccion { get; set; }
        public string? Correo { get; set; }
    }

    public class ConsultaHistorialDto
    {
        public int ConsultaId { get; set; }
        public DateTime FechaConsulta { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string? Diagnostico { get; set; }

        public int VeterinarioId { get; set; }
        public string VeterinarioNombre { get; set; } = string.Empty;
        public string EspecialidadVeterinario { get; set; } = string.Empty;

        public decimal CostoConsulta { get; set; }

        public List<TratamientoHistorialDto> Tratamientos { get; set; }
            = new();

        public decimal CostoTratamientos { get; set; }
        public decimal CostoTotalConsulta { get; set; }
    }

    public class TratamientoHistorialDto
    {
        public int TratamientoId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string? Medicamento { get; set; }
        public string? Dosis { get; set; }
        public int DuracionDias { get; set; }
        public decimal Costo { get; set; }
    }
}
