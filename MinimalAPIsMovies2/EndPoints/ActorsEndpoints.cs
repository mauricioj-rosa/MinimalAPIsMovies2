using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using MinimalAPIsMovies2.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using MinimalAPIsMovies2.Repositories;
using Microsoft.AspNetCore.OutputCaching;
using AutoMapper;
using MinimalAPIsMovies2.Entities;
using MinimalAPIsMovies2.Services;

namespace MinimalAPIsMovies2.EndPoints
{
    public static class ActorsEndpoints
    {
        private readonly static string container = "actors";
        public static RouteGroupBuilder MapActors(this RouteGroupBuilder group)
        {
            group.MapPost("/", Create).DisableAntiforgery();
            return group;
        }
        static async Task<Created<ActorDTO>> Create([FromForm] CreateActorDTO createActorDTO, IActorsRepository repository, IOutputCacheStore outputCacheStore, IMapper mapper, IFileStorage fileStorage)
        {
            var actor = mapper.Map<Actor>(createActorDTO);

            if (createActorDTO.Picture is not null)
            {
                var url = await fileStorage.Store(container, createActorDTO.Picture);
                actor.Picture = url;
            }
            var id = await repository.Create(actor);
            await outputCacheStore.EvictByTagAsync("actors-get", default);
            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Created($"/actors/{id}", actorDTO);
        }
    }
}
