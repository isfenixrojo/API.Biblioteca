using Microsoft.EntityFrameworkCore;

namespace API.Biblioteca.Entidades
{
    [PrimaryKey(nameof(IdAutor), nameof(IdLibro))]
    public class AutorLibro
    {
        public int IdAutor { get; set; }
        public int IdLibro { get; set; }
        public int Orden { get; set; }
        public Autor? Autor { get; set; }
        public Libro? Libro { get; set; }
    }
}
