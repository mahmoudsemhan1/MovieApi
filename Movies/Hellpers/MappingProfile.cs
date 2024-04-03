using AutoMapper;

namespace Movies.Hellpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MoviesDetaliesDto>();
            CreateMap<MovieDto, Movie>()
                .ForMember(src => src.Poster, opt => opt.Ignore());
        }
    }
}
