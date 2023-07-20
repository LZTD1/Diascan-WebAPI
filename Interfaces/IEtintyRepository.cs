using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IEntityRepository<T>
    {
        bool CreateEntity(T entity);
        bool UpdateEntity(T entity);
        bool DeleteEntity(T entity);
        bool DeleteEntities(IEnumerable<T> entities);
        bool Save();
    }
}
