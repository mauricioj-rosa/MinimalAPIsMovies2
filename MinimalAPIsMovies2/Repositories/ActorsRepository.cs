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
            return await context.Actors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<int> Create(Actor actor)
        {
            context.Actors.Add(actor);
            await context.SaveChangesAsync();
            return actor.Id; 
        }
        public async Task<bool> Exist(int id)
        {
            return await context.Actors.AnyAsync(a => a.Id == id);
        }
        public async Task Update(Actor actor)
        {
            context.Update(actor);
            await context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            await context.Actors.Where(a=> a.Id ==id).ExecuteDeleteAsync();
        }
    }
}

    

