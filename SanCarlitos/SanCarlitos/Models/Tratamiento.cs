using System;
using System.Collections.Generic;

namespace SanCarlitos.Models;

public partial class Tratamiento : BaseEntity
{


    public int ConsultaId { get; set; }

    public string Descripcion { get; set; } = null!;

    public string? Medicamento { get; set; }

    public string? Dosis { get; set; }

    public int DuracionDias { get; set; }

    public decimal Costo { get; set; }


    public virtual Consulta Consulta { get; set; } = null!;
}
