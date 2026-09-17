using System.ComponentModel.DataAnnotations;

namespace API.Biblioteca.DTOs
{
    public class ComentarioCreacionDTO
    {
        [Required]
        public required string Cuerpo { get; set; }
    }
}
