using API.Biblioteca.Entidades;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Biblioteca.DTOs
{
    public class LibroDTO
    {
        public int IdLibro { get; set; }
        public required string NombreLibro { get; set; }
    }
}
