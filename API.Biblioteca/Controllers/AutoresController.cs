using API.Biblioteca.Datos;
using API.Biblioteca.DTOs;
using API.Biblioteca.Entidades;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Biblioteca.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        #region Dependencias
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion Dependencias

        [HttpGet]
        public async Task<IEnumerable<AutorDTO>> GetAutores()
        {
            var autores = await _context.Autores.ToListAsync();
            var autorerDTO = _mapper.Map<IEnumerable<AutorDTO>>(autores);
            return autorerDTO;
        }

        [HttpGet("{idAutor:int}", Name = "GetAutorById")]
        public async Task<ActionResult<AutorLibrosDTO>> GetAutorById(int idAutor)
        {
            var autor = await _context.Autores
                .Include(x => x.Libros)
                .ThenInclude(x=> x.Libro)
                .FirstOrDefaultAsync(x => x.IdAutor == idAutor);
            if (autor == null)
            {
                return NotFound();
            }
            var autorDTO = _mapper.Map<AutorLibrosDTO>(autor);
            return autorDTO;
        }

        [HttpPost]
        public async Task<ActionResult> InsertAutor([FromBody] CreateAutorDTO createAutorDTO)
        {
            var autor = _mapper.Map<Autor>(createAutorDTO);
            _context.Add(autor);
            await _context.SaveChangesAsync();
            var autorDTO = _mapper.Map<AutorDTO>(autor);
            return CreatedAtRoute("GetAutorById", new { idAutor = autor.IdAutor }, autorDTO);
        }

        [HttpPut("{idAutor:int}")]
        public async Task<ActionResult> PutAutor(int idAutor, CreateAutorDTO createAutorDTO)
        {
            /*if (idAutor != autor.IdAutor)
            {
                return BadRequest("El Id del Autor no coincide con el registro.");
            }*/

            var autor = _mapper.Map<Autor>(createAutorDTO);
            autor.IdAutor = idAutor;
            _context.Update(autor);
            await _context.SaveChangesAsync();
            return Ok("Success");
        }

        [HttpPatch("{idAutor:int}")]
        public async Task<ActionResult> PatchAutor(int idAutor, JsonPatchDocument<AutorPatchDTO> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest();
            }
            var autorDB = await _context.Autores.FirstOrDefaultAsync(x => x.IdAutor == idAutor);

            if (autorDB is null)
            {
                return NotFound();
            }
            var autorPatchDTO = _mapper.Map<AutorPatchDTO>(autorDB);
            patchDoc.ApplyTo(autorPatchDTO, ModelState);
            var esValido = TryValidateModel(autorPatchDTO);

            if (!esValido)
            {
                return ValidationProblem();
            }
            _mapper.Map(autorPatchDTO, autorDB);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{idAutor:int}")]
        public async Task<ActionResult> DeleteAutor(int idAutor)
        {
            var registrosBorrados = await _context.Autores.Where(x => x.IdAutor == idAutor).ExecuteDeleteAsync();
            if (registrosBorrados == 0)
            {
                return NotFound();
            }
            return Ok("Success");
        }
    }
}
