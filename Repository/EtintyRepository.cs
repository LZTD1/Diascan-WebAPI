using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class EntityRepository<T> : IEntityRepository<T> 
    {
        private readonly DataContext _context;
        public EntityRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreateEntity(T entity)
        {
            _context.Add(entity);
            return Save();
        }

        public bool DeleteEntity(T entity)
        {
            _context.Remove(entity);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateEntity(T entity)
        {
            _context.Update(entity);
            return Save();
        }
        public bool DeleteEntities(IEnumerable<T> entities)
        {
            _context.RemoveRange(entities);
            return Save();
        }
    }
}
