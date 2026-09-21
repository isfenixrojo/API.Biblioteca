using API.Biblioteca.Datos;
using API.Biblioteca.DTOs;
using API.Biblioteca.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Biblioteca.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        #region Dependencias 
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion Dependencias

        [HttpGet]
        public async Task<ActionResult> GetLibros()
        {
            var libros = await _context.Libros.ToListAsync();

            if (libros.Count == 0)
            {
                return NotFound("Sin datos.");
            }
            var libroDTO = _mapper.Map<IEnumerable<LibroDTO>>(libros);
            return Ok(libroDTO);
        }

        [HttpGet("{idLibros:int}", Name = "GetLibroById")]
        public async Task<ActionResult> GetLibroById(int idLibros)
        {
            var libros = await _context.Libros
                .Include(x => x.Autores)
                .FirstOrDefaultAsync(x => x.IdLibro == idLibros);
            if (libros == null)
            {
                return NotFound("Sin datos.");
            }
            var autorDTO = _mapper.Map<LibroAutorDTO>(libros);
            return Ok(autorDTO);
        }


        [HttpPost]
        public async Task<ActionResult> PostLibro(CreateLibroDTO createLibroDTO)
        {
            if (createLibroDTO.AutoresIds is null || createLibroDTO.AutoresIds.Count == 0)
            {
                ModelState.AddModelError(nameof(createLibroDTO.AutoresIds),
                    "No se puede crear un libro sin autores");
                return ValidationProblem();
            }

            var autoresIdsExisten = await _context.Autores
                                    .Where(x => createLibroDTO.AutoresIds.Contains(x.IdAutor))
                                    .Select(x => x.IdAutor).ToListAsync();


            if (autoresIdsExisten.Count != createLibroDTO.AutoresIds.Count)
            {
                var autoresNoExisten = createLibroDTO.AutoresIds.Except(autoresIdsExisten);
                var autoresNoExistenString = string.Join(", ", autoresNoExisten);
                var mensajeDeError = $"Los siguentes autores no existen: {autoresNoExistenString}";
                ModelState.AddModelError(nameof(createLibroDTO.AutoresIds), mensajeDeError);

                return ValidationProblem();
            }

            var libro = _mapper.Map<Libro>(createLibroDTO);

            AsignarOrdenAutores(libro);

            _context.Add(libro);
            await _context.SaveChangesAsync();

            var libroDTO = _mapper.Map<LibroDTO>(libro);
            return CreatedAtRoute("GetLibroById", new { idLibros = libro.IdLibro }, libroDTO);
        }

        private void AsignarOrdenAutores(Libro libro)
        {
            if (libro.Autores is not null)
            {
                for (int i = 0; i < libro.Autores.Count; i++)
                {
                    libro.Autores[i].Orden = i;
                }
            }
        }

        /*
        [HttpPut("{idLibros:int}")]
        public async Task<ActionResult> PutLibro(int idLibros, CreateLibroDTO createLibroDTO)
        {
            var libro = _mapper.Map<Libro>(createLibroDTO);

            /*if (idLibros != libro.IdLibro)
            {
                return BadRequest("Los IDs deben coincidir.");
            }*/
        /*
            libro.IdLibro = idLibros;   
            var existeAutor = await _context.Autores.AnyAsync(x => x.IdAutor == libro.IdAutor);
            if (!existeAutor)
            {
                return BadRequest($"El autor de id {libro.IdAutor} no existe.");
            }

            var existeLibro = await _context.Libros.AnyAsync(x => x.IdLibro == idLibros);
            if (!existeLibro)
            {
                return BadRequest($"El libro con id {libro.IdLibro} no existe.");
            }

            _context.Update(libro);
            await _context.SaveChangesAsync();
            return Ok("Success");
        }*/

        [HttpDelete("{idLibro:int}")]
        public async Task<ActionResult> DelteLibro(int idLibro)
        {
            var registroBorrado = await _context.Libros.Where(x => x.IdLibro == idLibro).ExecuteDeleteAsync();
            if (registroBorrado == 0)
            {
                return NotFound();
            }
            return Ok("Success");

        }

    }
}
