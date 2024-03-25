
using Microsoft.EntityFrameworkCore;
using WorkoutApp.Entities;

namespace WorkoutApp.Repositories
{
    public class SqlRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private static readonly string LogFilePath = "workoutsLog.log";

        private readonly DbSet<T> _dbSet;
        private readonly DbContext _dbContext;

        public SqlRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public event EventHandler<T>? WorkoutAdded;
        public event EventHandler<T>? WorkoutRemoved;
        public event EventHandler<T>? WorkoutLastUsed;

        public void AddWorkout(T item)
        {
            _dbSet.Add(item);
            WorkoutAdded?.Invoke(this, item);
            LogAudit($"WorkoutAdded {typeof(T).Name} => {item}");
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void LastUsedWorkout(T item, DateTime lastUsedDate)
        {
            if (item is Workout workout)
            {
                workout.LastUsed = lastUsedDate;
            }

            WorkoutLastUsed?.Invoke(this, item);
            LogAudit($"WorkoutLastUsed updated {typeof(T).Name} => {item}");
        }

        public void RemoveWorkout(T item)
        {
            _dbSet.Remove(item);
            WorkoutRemoved?.Invoke(this, item);
            LogAudit($"WorkoutRemoved {typeof(T).Name} => {item}");
        }

        public void SaveWorkout()
        {
            _dbContext.SaveChanges();
        }
        public void LogAudit(string logEntry)
        {
            File.AppendAllText(LogFilePath, $"[{DateTime.Now:dd.MM.yyyy - HH:mm:ss}] - {logEntry}" + Environment.NewLine);
        }
    }
}
