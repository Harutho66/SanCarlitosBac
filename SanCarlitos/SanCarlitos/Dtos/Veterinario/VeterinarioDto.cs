namespace SanCarlitos.Dtos.Veterinario
{
    public class VeterinarioDto
    {
        public int id {  get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public string? Celular { get; set; }
        public string MatriculaProfesional { get; set; } = string.Empty;
    }
}
