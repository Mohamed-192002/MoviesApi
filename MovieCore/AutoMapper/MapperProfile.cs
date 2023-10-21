using AutoMapper;
using MoviesApi.DTOS;
using MoviesApi.Models;

namespace MoviesApi.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Genres
            CreateMap<GenresDto, Genre>();

            // Movies
            CreateMap<Movie, MovieDetailsDto>();
            CreateMap<MovieCreateDto, Movie>()
                .ForMember(src => src.Poster, opt => opt.Ignore());
            CreateMap<MovieUpdateDto, Movie>();
        }
    }
}
