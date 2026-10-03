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
            CreateMap<AutorLibro, LibroDTO>()
                .ForMember(dto => dto.IdLibro, config => config.MapFrom(ent=> ent.IdLibro))
                .ForMember(dto => dto.NombreLibro, config => config.MapFrom(ent => ent.Libro!.NombreLibro));

            CreateMap<Libro, LibroAutorDTO>();

            CreateMap<AutorLibro, AutorDTO>()
                .ForMember(dto => dto.IdAutor, config => config.MapFrom(ent => ent.IdAutor))
                .ForMember(dto => dto.NombreCompletoAutor,
                config => config.MapFrom(ent => MapearNombreApellidos(ent.Autor!)));

            CreateMap<CreateLibroDTO, AutorLibro>()
                .ForMember(ent => ent.Libro,
                config => config.MapFrom(dto => new Libro {NombreLibro = dto.NombreLibro }));

            CreateMap<Libro, LibroDTO>();
            CreateMap<CreateLibroDTO, Libro>().ForMember(ent => ent.Autores, config =>
            config.MapFrom(dto => dto.AutoresIds.Select(id => new AutorLibro { IdAutor = id })));

            

            CreateMap<ComentarioCreacionDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();
            CreateMap<ComentarioPatchDTO, Comentario>().ReverseMap();
        }

        private string MapearNombreApellidos(Autor autor) => $"{autor!.NombreAutor} {autor.Apellidos}";

    }
}