namespace API.Biblioteca.DTOs
{
    public class AutorLibrosDTO : AutorDTO
    {
        public List<LibroDTO> Libros { get; set; } = [];
    }
}
