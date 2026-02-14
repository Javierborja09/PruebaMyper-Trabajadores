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

    public string? Direccion { get; set; }

    public int? IdDistrito { get; set; }

    public string? FotoRuta { get; set; }

    public DateTime? FechaCreacion { get; set; }

    // Nota: Solo incluimos IdDistrito por un tema de orden y respeto a la normalización. 
    // No pusimos campos de Provincia o Departamento porque, técnicamente, ya están 
    // conectados a traves del Distrito. 
    // Si los pusiéramos aqui, tendríamos información duplicada en todos lados y seriaa 
    // un dolor de cabeza mantener todo sincronizado. Es mejor dejar que el Distrito 
    // sea la puerta de enlace para traer el resto cuando realmente lo necesitemos.
    public virtual Distrito? IdDistritoNavigation { get; set; }
}
