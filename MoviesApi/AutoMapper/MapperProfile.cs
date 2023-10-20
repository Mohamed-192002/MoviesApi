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
            //CreateMap<Movie,MoviesDto>()
            //    .ForMember(src=>src.Poster,opt=>opt.Ignore())
            //    .ReverseMap();
        }
    }
}
