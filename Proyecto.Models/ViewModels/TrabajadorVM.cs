using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models.ViewModels
{
    public class TrabajadorVM
    {
        public int IdTrabajador { get; set; }

        [Required(ErrorMessage = "Seleccione el tipo de documento")]
        public string TipoDocumento { get; set; } = null!;

        [Required(ErrorMessage = "El número de documento es obligatorio")]
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres")]
        public string NumeroDocumento { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese los nombres")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese los apellidos")]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "Seleccione el sexo")]
        public string Sexo { get; set; } = null!;

        [StringLength(255, ErrorMessage = "La dirección es demasiado larga")]
        public string? Direccion { get; set; }

        public string? FotoRuta { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public string? NombreDistrito { get; set; }
        public string? NombreProvincia { get; set; }
        public string? NombreDepartamento { get; set; }

        public string NombreCompleto => $"{Nombres} {Apellidos}";
    }
}