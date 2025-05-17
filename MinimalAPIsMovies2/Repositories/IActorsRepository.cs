using MinimalAPIsMovies2.Entities;

namespace MinimalAPIsMovies2.Repositories
{
    public interface IActorsRepository
    {
        Task<List<Actor>> GetAll();
        Task<Actor?> GetById(int id);
    }
}