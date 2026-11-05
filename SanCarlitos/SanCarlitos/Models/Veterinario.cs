using System;
using System.Collections.Generic;

namespace SanCarlitos.Models;

public partial class Veterinario : BaseEntity
{

    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Especialidad { get; set; } = null!;
    public string? Celular { get; set; }
    public string MatriculaProfesional { get; set; } = null!;
    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();
}
