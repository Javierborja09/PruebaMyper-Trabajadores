using System;
using System.Collections.Generic;

namespace Proyecto.Datos.DataContext;

public partial class Distrito
{
    public int IdDistrito { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdProvincia { get; set; }

    public virtual Provincia? IdProvinciaNavigation { get; set; }

    public virtual ICollection<Trabajadore> Trabajadores { get; set; } = new List<Trabajadore>();
}
