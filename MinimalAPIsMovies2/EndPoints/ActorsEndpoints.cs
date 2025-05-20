using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using MinimalAPIsMovies2.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MinimalAPIsMovies2.EndPoints
{
    public static class ActorsEndpoints
    {
        public static RouteGroupBuilder MapActors(this RouteGroupBuilder group)
        {
            return group;
        }
        static async Task<Created<ActorDTO>> Create([FromForm])
    }
}
