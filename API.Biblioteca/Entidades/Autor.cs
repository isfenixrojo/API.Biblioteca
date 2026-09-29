using API.Biblioteca.Validaciones;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Biblioteca.Entidades
{
    public class Autor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAutor { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(70, MinimumLength = 4, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos.")]
        public required string NombreAutor { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos.")]
        public required string Apellidos { get; set; }

        [StringLength(50, MinimumLength = 4, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos.")]
        public string? Identificacion { get; set; }
        public List<AutorLibro> Libros { get; set; } = [];
    }
}
