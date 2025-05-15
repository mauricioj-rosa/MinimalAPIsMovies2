using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies2.DTOs;
using MinimalAPIsMovies2.Entities;
using MinimalAPIsMovies2.Migrations;
using MinimalAPIsMovies2.Repositories;

namespace MinimalAPIsMovies2.EndPoints
{
    public static class GenresEndpoints
    {
        public static RouteGroupBuilder MapGenres(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetGenres).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("genres-get"));
            group.MapGet("/{id:int}", GetById);
            group.MapPost("/", Create);
            group.MapPut("/{id:int}", Update);
            group.MapDelete("/{id:int}", Delete);

            return group;
        }



        static async Task<Ok<List<GenreDTO>>> GetGenres(IGenresRepository repository)
        {
            var genres = await repository.GetAll();
            var genresDTO = genres.Select(g => new GenreDTO { Id = g.Id, Name = g.Name }).ToList();
            
            return TypedResults.Ok(genresDTO);
        }

        static async Task<Results<Ok<GenreDTO>, NotFound>> GetById(int id, IGenresRepository repository)
        {
            var genre = await repository.GetById(id);

            if (genre is null)
            {
                return TypedResults.NotFound();
            }
            var genreDTO = new GenreDTO
            {
                Id = genre.Id,
                Name = genre.Name,
            };
            return TypedResults.Ok(genreDTO);
        }

        static async Task<Created<GenreDTO>> Create(CreateGenreDTO createGenreDTO, IGenresRepository repository, IOutputCacheStore outputCacheStore)
        {
            var genre = new Genre
            {
                Name= createGenreDTO.Name
            };
            var genreDTO = new GenreDTO
            {
                Id = genre.Id,
                Name = genre.Name,
            };
            var id = await repository.Create(genre);
            await outputCacheStore.EvictByTagAsync("genres-get", default);
            return TypedResults.Created($"/{id}", genreDTO);
        }

        static async Task<Results<NotFound, NoContent>> Update(int id, CreateGenreDTO createGenreDTO, IGenresRepository repository, IOutputCacheStore outputCacheStore)
        {
            var exists = await repository.Exists(id);

            if (!exists)
            {
                return TypedResults.NotFound();
            }
            var genre = new Genre
            {
                Name = createGenreDTO.Name
            };
            await repository.Update(genre);
            await outputCacheStore.EvictByTagAsync("genre-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent>> Delete(int id, IGenresRepository repository, IOutputCacheStore outputCacheStore)
        {
            var exists = await repository.Exists(id);

            if (!exists)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
            await outputCacheStore.EvictByTagAsync("genres-get", default);
            return TypedResults.NoContent();
        }
    }
}
