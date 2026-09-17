using System.ComponentModel.DataAnnotations;

namespace API.Biblioteca.DTOs
{
    public class AutorDTO
    {
        public int IdAutor { get; set; }
        public required string NombreCompletoAutor { get; set; }
    }
}
