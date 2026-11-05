using System;
using System.Collections.Generic;

namespace SanCarlitos.Models;

public partial class Propietario : BaseEntity
{
    

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Celular { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Correo { get; set; }


    public virtual ICollection<Mascota> Mascota { get; set; } = new List<Mascota>();
}
