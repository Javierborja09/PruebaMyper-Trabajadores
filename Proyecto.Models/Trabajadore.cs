using System;
using System.Collections.Generic;

namespace Proyecto.Models;
public partial class Trabajadore
{
    public int IdTrabajador { get; set; }

    public string TipoDocumento { get; set; } = null!;

    public string NumeroDocumento { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Sexo { get; set; } = null!;

    public int? IdDistrito { get; set; }

    public string? FotoRuta { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Distrito? IdDistritoNavigation { get; set; }
}
