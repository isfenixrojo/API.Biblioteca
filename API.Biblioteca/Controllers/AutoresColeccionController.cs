using API.Biblioteca.Datos;
using API.Biblioteca.DTOs;
using API.Biblioteca.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;

namespace API.Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresColeccionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AutoresColeccionController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{idAutores}", Name = "GetAutoresById")]
        public async Task<ActionResult<List<AutorLibrosDTO>>> GetAutoresById([FromRoute] string idAutores)
        {
            var idsColecion = new List<int>();  

            foreach (var id in idAutores.Split(','))
            {
                if (int.TryParse(id, out int idAutor))
                {
                    idsColecion.Add(idAutor);
                }
            }

            if (!idsColecion.Any())
            {
                ModelState.AddModelError(nameof(idAutores), "Ningun Id fue encontrado");
                return ValidationProblem();
            }
            var autores = await _context.Autores
                .Include(x=> x.Libros)
                .ThenInclude(x=> x.Libro)
                .Where(x=> idsColecion.Contains(x.IdAutor))
                .ToListAsync(); 

            if (autores.Count != idsColecion.Count) 
            {
                return NotFound();  
            }
            var autoresDTO = _mapper.Map<List<AutorLibrosDTO>>(autores);
            return autoresDTO;  

        }   

        [HttpPost]
        public async Task<ActionResult> PostAutoresColeccion([FromBody] IEnumerable<CreateAutorDTO> createAutorDTO)
        {
            var autores = _mapper.Map<IEnumerable<Autor>>(createAutorDTO);
            _context.AddRange(autores);
            await _context.SaveChangesAsync();
            var autoresDTO = _mapper.Map<IEnumerable<AutorDTO>>(autores);

            var idsAutores = autores.Select(x => x.IdAutor);
            var idsString = string.Join(",", idsAutores);
               
            return CreatedAtRoute("GetAutoresById", new { idAutores = idsString}, autoresDTO);
        }   
    }
}
