namespace API.Biblioteca.DTOs
{
    public class LibroAutorDTO : LibroDTO
    {
        public int IdAutor { get; set; }
        public required string NombreAutor { get; set; }
    }
}
