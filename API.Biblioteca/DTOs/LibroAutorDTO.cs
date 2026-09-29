namespace API.Biblioteca.DTOs
{
    public class LibroAutorDTO : LibroDTO
    {
        public List<AutorDTO> Autores { get; set; } = [];
        
    }
}
