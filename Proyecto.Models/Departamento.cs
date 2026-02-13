using System;
using System.Collections.Generic;

namespace Proyecto.Models;

public partial class Departamento
{
    public int IdDepartamento { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Provincia> Provincia { get; set; } = new List<Provincia>();
}
