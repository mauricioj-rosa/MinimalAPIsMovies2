using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies2.Entities;
namespace MinimalAPIsMovies2
{
    public class ApplicationDbContext : DbContext
    
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {

        }

        public DbSet<Genre> Genres { get; set; }
    }
}
