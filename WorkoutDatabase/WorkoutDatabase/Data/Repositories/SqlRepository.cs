
using Microsoft.EntityFrameworkCore;
using WorkoutDatabase.Components.CsvReader;
using WorkoutDatabase.Entities;

namespace WorkoutDatabase.Data.Repositories
{
    public class SqlRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private static readonly string LogFilePath = "workoutsSqlLog.log";

        private readonly DbSet<T> _workoutDbSet;
        private readonly WorkoutDbContext _workoutDbContext;
        public SqlRepository(WorkoutDbContext dbContext)
        {
            _workoutDbContext = dbContext;
            _workoutDbSet = _workoutDbContext.Set<T>();
        }

        public event EventHandler<T>? WorkoutAdded;
        public event EventHandler<T>? WorkoutRemoved;
        public event EventHandler<T>? WorkoutLastUsed;

        public void AddWorkout(T item)
        {
            _workoutDbContext.Add(item);
            SaveWorkout();
            WorkoutAdded?.Invoke(this, item);
            LogAudit($"WorkoutAdded {typeof(T).Name} => {item}");
        }

        public IEnumerable<T> GetAll()
        {
            return _workoutDbSet.OrderBy(item => item.Id).ToList();
        }

        public T? GetById(int id)
        {
            return _workoutDbSet.Find(id);
        }

        public void LastUsedWorkout(T item, DateTime lastUsedDate)
        {
            if (item is WorkoutEntities workout)
            {
                workout.LastUsed = lastUsedDate;
            }

            SaveWorkout();
            WorkoutLastUsed?.Invoke(this, item);
            LogAudit($"WorkoutLastUsed updated {typeof(T).Name} => {item}");
        }

        public void RemoveWorkout(T item)
        {
            _workoutDbContext.Remove(item);
            SaveWorkout();
            WorkoutRemoved?.Invoke(this, item);
            LogAudit($"WorkoutRemoved {typeof(T).Name} => {item}");
        }  

        public virtual void SaveWorkout()
        {
            try
            {
                _workoutDbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error occurred while saving: {exception.Message}");
            }
        }

        public void LogAudit(string logEntry)
        {
            File.AppendAllText(LogFilePath, $"[{DateTime.Now:dd.MM.yyyy - HH:mm:ss}] - {logEntry}" + Environment.NewLine);
        }
    }
}
