using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies2.Entities;
using MinimalAPIsMovies2.Repositories;

namespace MinimalAPIsMovies2.Repositories
{
    public class GenresRepository : IGenresRepository
    {
        private readonly ApplicationDbContext context;

        public GenresRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<int> Create(Genre genre)
        {
            context.Add(genre);
            await context.SaveChangesAsync();
            return genre.Id;
        }

        public async Task<bool> Exists(int id)
        {
            return await context.Genres.AnyAsync(g=>g.Id==id);
        }

        public async Task<List<Genre>> GetAll()
        {
            return await context.Genres.OrderBy(g=>g.Name).ToListAsync();
        }

        public async Task<Genre?> GetById(int id)
        {
            return await context.Genres.FirstOrDefaultAsync(g=> g.Id == id);
        }

        public async Task Update(Genre genre)
        {
            context.Update(genre);
            await context.SaveChangesAsync();

        }
    }
}
