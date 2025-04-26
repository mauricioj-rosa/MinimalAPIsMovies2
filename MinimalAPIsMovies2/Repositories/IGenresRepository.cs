using MinimalAPIsMovies2.Entities;

namespace MinimalAPIsMovies2.Repositories
{
    public interface IGenresRepository
    {
        Task<int> Create(Genre genre);
        Task<Genre?> GetById(int id);
        Task<List<Genre>> GetAll();
    }
}
