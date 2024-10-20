
using Microsoft.EntityFrameworkCore;
using WorkoutDatabase.DataAccess.Data;
using WorkoutDatabase.DataAccess.Data.Entities;

namespace WorkoutDatabase.ApplicationServices.Repositories
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

        public event EventHandler<T>? ItemAdded;
        public event EventHandler<T>? ItemRemoved;
        public event EventHandler<T>? ItemLastUsed;

        public void AddItem(T item)
        {
            _workoutDbContext.Add(item);
            SaveItem();
            ItemAdded?.Invoke(this, item);
            LogAudit($"Item Added {typeof(T).Name} => {item}");
        }

        public IEnumerable<T> GetAll()
        {
            LogAudit("Items ListTaken");
            return _workoutDbSet.OrderBy(item => item.Id).ToList();
        }

        public T? GetById(int id)
        {
            LogAudit($"Items LookedFor {typeof(T).Name} => {id}");
            return _workoutDbSet.Find(id);
        }

        public void UpdateItem(T item, DateTime lastUsedDate)
        {
            if (item is WorkoutEntity workout)
            {
                workout.LastUsed = lastUsedDate;
            }
            SaveItem();
            ItemLastUsed?.Invoke(this, item);
            LogAudit($"Item Updated {typeof(T).Name} => {item}");
        }

        public void RemoveItem(T item)
        {
            _workoutDbContext.Remove(item);
            SaveItem();
            ItemRemoved?.Invoke(this, item);
            LogAudit($"Item Removed {typeof(T).Name} => {item}");
        }

        public virtual void SaveItem()
        {
            try
            {
                _workoutDbContext.SaveChanges();
                LogAudit("Items Saved");
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

        public void EditItem(int id, Action<T> editAction)
        {
            var item = _workoutDbSet.Find(id);
            editAction(item);
            SaveItem();
            LogAudit($"Item edited: {typeof(T).Name} => {item}");
        }
    }
}
