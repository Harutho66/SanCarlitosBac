namespace SanCarlitos.Dtos.AtencionesDelDia
{
    public class MascotaAtencionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string? Raza { get; set; }
    }

    public class PropietarioAtencionDto
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
    }

    public class VeterinarioAtencionDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
    }

    public class TratamientoAtencionDto
    {
        public string Descripcion { get; set; } = string.Empty;
        public string? Medicamento { get; set; }
        public string? Dosis { get; set; }
        public int DuracionDias { get; set; }
        public decimal Costo { get; set; }
    }
}
