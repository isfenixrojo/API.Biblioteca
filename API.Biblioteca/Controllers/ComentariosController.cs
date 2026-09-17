using API.Biblioteca.Datos;
using API.Biblioteca.DTOs;
using API.Biblioteca.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Biblioteca.Controllers
{
    [ApiController]
    [Route("api/libros/{idLibro:int}/comentarios")]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ComentariosController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> GetComentarioById(int idLibro)
        {
            var exiteLibro = await _context.Libros.AnyAsync(x => x.IdLibro == idLibro);
            if (!exiteLibro)
            {
                return NotFound();
            }

            var comentarios = await _context.Comentarios
                .Where(x => x.LibroId == idLibro)
                .OrderByDescending(x => x.FechaPublicacion)
                .ToListAsync();

            if (comentarios.Count == 0)
            {
                return NotFound("Sin datos");
            }

            return _mapper.Map<List<ComentarioDTO>>(comentarios);
        }

        [HttpGet("{id}", Name = "ObtenerComentarioGuid")]
        public async Task<ActionResult<ComentarioDTO>> GetComentariosGuid(Guid id)
        {
            var comentario = await _context.Comentarios.FirstOrDefaultAsync(x => x.Id == id);
            if (comentario is null)
            {
                return NotFound();
            }
            return _mapper.Map<ComentarioDTO>(comentario);
        }


        [HttpPost]
        public async Task<ActionResult> PostComentario(int idLibro, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var exiteLibro = await _context.Libros.AnyAsync(x => x.IdLibro == idLibro);
            if (!exiteLibro)
            {
                return NotFound();
            }

            var comentario = _mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = idLibro;
            comentario.FechaPublicacion = DateTime.UtcNow;
            _context.Add(comentario);
            await _context.SaveChangesAsync();

            var comentarioDTO = _mapper.Map<ComentarioDTO>(comentario);

            return CreatedAtRoute("ObtenerComentarioGuid", new { id = comentario.Id, idLibro }, comentarioDTO);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchAutor(Guid id, int idLibro, JsonPatchDocument<ComentarioPatchDTO> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest();
            }

            var exiteLibro = await _context.Libros.AnyAsync(x => x.IdLibro == idLibro);
            if (!exiteLibro)
            {
                return NotFound();
            }

            var comentarioDB = await _context.Comentarios.FirstOrDefaultAsync(x => x.Id == id);

            if (comentarioDB is null)
            {
                return NotFound();
            }
            var comentarioPatchDTO = _mapper.Map<ComentarioPatchDTO>(comentarioDB);
            patchDoc.ApplyTo(comentarioPatchDTO, ModelState);

            var esValido = TryValidateModel(comentarioPatchDTO);
            if (!esValido)
            {
                return ValidationProblem();
            }
            _mapper.Map(comentarioPatchDTO, comentarioDB);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComentario(Guid id, int idLibro)
        {
            var exiteLibro = await _context.Libros.AnyAsync(x => x.IdLibro == idLibro);
            if (!exiteLibro)
            {
                return NotFound();
            }

            var registrosBorrados = await _context.Comentarios.Where(x => x.Id == id).ExecuteDeleteAsync();

            if (registrosBorrados == 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        /*[HttpGet]   
        public async Task<ActionResult<IEnumerable<ComentarioDTO>>> GetComentarios()
        {
            var comentario = await _context.Comentarios.ToListAsync();
            var comentarioDTO = _mapper.Map<ComentarioDTO>(comentario);
            return Ok(comentarioDTO);
        }*/
    }
}
