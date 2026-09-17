using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Biblioteca.DTOs
{
    public class CreateLibroDTO
    {
        [Required(ErrorMessage = "El nombre del libro es obligatorio.")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "El NombreAutor debe tener entre 20 y 30 caracteres.")]
        public required string NombreLibro { get; set; }
        public int IdAutor { get; set; }
    }
}
