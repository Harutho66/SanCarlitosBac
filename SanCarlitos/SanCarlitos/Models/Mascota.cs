using System;
using System.Collections.Generic;

namespace SanCarlitos.Models;

public partial class Mascota : BaseEntity
{
    public int PropietarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Especie { get; set; } = null!;

    public string? Raza { get; set; }

    public string Sexo { get; set; } = null!;

    public DateOnly? FechaNacimiento { get; set; }


    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    public virtual Propietario Propietario { get; set; } = null!;
}
