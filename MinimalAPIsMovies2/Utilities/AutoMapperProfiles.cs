using AutoMapper;
using MinimalAPIsMovies2.DTOs;
using MinimalAPIsMovies2.Entities;

namespace MinimalAPIsMovies2.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Genre, GenreDTO>();
            CreateMap<CreateGenreDTO, Genre>();
        }
    }
}
