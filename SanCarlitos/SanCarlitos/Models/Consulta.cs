using System;
using System.Collections.Generic;

namespace SanCarlitos.Models;

public partial class Consulta: BaseEntity
{

    public int MascotaId { get; set; }

    public int VeterinarioId { get; set; }

    public DateTime FechaConsulta { get; set; }

    public string Motivo { get; set; } = null!;

    public string? Diagnostico { get; set; }

    public decimal Costo { get; set; }



    public virtual Mascota Mascota { get; set; } = null!;

    public virtual ICollection<Tratamiento> Tratamientos { get; set; } = new List<Tratamiento>();

    public virtual Veterinario Veterinario { get; set; } = null!;
}
