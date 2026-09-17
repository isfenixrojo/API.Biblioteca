using API.Biblioteca.DTOs;
using API.Biblioteca.Entidades;
using AutoMapper;

namespace API.Biblioteca.Utilidades
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Autor, AutorDTO>()
                .ForMember(dto => dto.NombreCompletoAutor,
                    config => config.MapFrom(autor => MapearNombreApellidos(autor)));

            CreateMap<Autor, AutorLibrosDTO>()
               .ForMember(dto => dto.NombreCompletoAutor,
                   config => config.MapFrom(autor => MapearNombreApellidos(autor)));

            CreateMap<CreateAutorDTO, Autor>();
            CreateMap<Autor, AutorPatchDTO>().ReverseMap();

            CreateMap<Libro, LibroDTO>();

            CreateMap<CreateLibroDTO, Libro>();

            /*CreateMap<Libro, LibroAutorDTO>()
                .ForMember(dto => dto.NombreAutor, config =>
                                config.MapFrom(ent => MapearNombreApellidos(ent.Autor!)));*/

            CreateMap<ComentarioCreacionDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();
            CreateMap<ComentarioPatchDTO, Comentario>().ReverseMap();   
        }

        private string MapearNombreApellidos(Autor autor) => $"{autor!.NombreAutor} {autor.Apellidos}";

    }
}