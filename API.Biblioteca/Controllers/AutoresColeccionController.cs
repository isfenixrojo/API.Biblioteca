using API.Biblioteca.Datos;
using API.Biblioteca.DTOs;
using API.Biblioteca.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<ActionResult> PostAutoresColeccion([FromBody] IEnumerable<CreateAutorDTO> createAutorDTO)
        {
            var autores = _mapper.Map<IEnumerable<Autor>>(createAutorDTO);
            _context.AddRange(autores);
            await _context.SaveChangesAsync();
            return Ok();
        }   
    }
}
