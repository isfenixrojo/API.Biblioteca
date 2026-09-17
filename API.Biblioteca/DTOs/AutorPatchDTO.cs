using System.ComponentModel.DataAnnotations;

namespace API.Biblioteca.DTOs
{
    public class AutorPatchDTO
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(70, MinimumLength = 4, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos.")]
        public required string NombreAutor { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos.")]
        public required string Apellidos { get; set; }

        [StringLength(50, MinimumLength = 4, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos.")]
        public string? Identificacion { get; set; }
    }
}
