using Microsoft.EntityFrameworkCore;
using WorkoutDatabase.Entities;

namespace WorkoutDatabase.Repositories
{
    //public delegate void ItemAdded<in T>(T item);
    public class SqlRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly DbSet<T> _dbSet;
        private readonly DbContext _dbContext;
        private readonly Action<T>? _itemAddedCallback;

        public SqlRepository(DbContext dbContext, Action<T>? itemAddedCallback = null)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
            _itemAddedCallback = itemAddedCallback;
        }

        public event EventHandler<T>? ItemAdded;

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void AddWorkout(T item)
        {
            _dbSet.Add(item);
            _itemAddedCallback?.Invoke(item);
            ItemAdded?.Invoke(this, item);
        }

        public void RemoveWorkout(T item)
        {
            _dbSet.Remove(item);
        }

        public void SaveWorkout()
        {
            _dbContext.SaveChanges();
        }
    }
}
