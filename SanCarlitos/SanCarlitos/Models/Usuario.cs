using System;
using System.Collections.Generic;

namespace SanCarlitos.Models;

public partial class Usuario : BaseEntity
{
    public string NombreUsuario { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public bool Activo { get; set; }
}
