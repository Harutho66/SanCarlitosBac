namespace SanCarlitos.Dtos.Propietario
{
    public class PropietarioDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Celular { get; set; } = string.Empty;

        public string? Direccion { get; set; }

        public string? Correo { get; set; }

        public DateTime FechaRegistro { get; set; }

    }
}
