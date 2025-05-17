using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies2.Entities;
using MinimalAPIsMovies2.Migrations;


namespace MinimalAPIsMovies2.Repositories
{
    public class ActorsRepository(ApplicationDbContext context) : IActorsRepository
    {
        public async Task<List<Actor>> GetAll()
        {
            return await context.Actors.OrderBy(a => a.Name).ToListAsync();
        }
        public async Task<Actor?> GetById(int id)
        {
            return await context.Actors.FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}

    

