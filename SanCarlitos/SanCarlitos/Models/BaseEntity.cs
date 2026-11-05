namespace SanCarlitos.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public bool Eliminado { get; set; } = false;

        public DateTime FechaRegistro { get; set; } = 
            DateTime.UtcNow.AddHours(-4);

    }
}
