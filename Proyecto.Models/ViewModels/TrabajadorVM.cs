using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models.ViewModels
{
    public class TrabajadorVM
    {
        public int IdTrabajador { get; set; }

        public string TipoDocumento { get; set; } = null!;
        public string NumeroDocumento { get; set; } = null!;

        public string Nombres { get; set; } = null!;

        public string Apellidos { get; set; } = null!;

        public string Sexo { get; set; } = null!;
        public string? Direccion { get; set; }

        public string? FotoRuta { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public string? NombreDistrito { get; set; }
        public string? NombreProvincia { get; set; }
        public string? NombreDepartamento { get; set; }

        public string NombreCompleto => $"{Nombres} {Apellidos}";
    }
}