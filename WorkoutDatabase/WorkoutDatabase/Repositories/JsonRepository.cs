
using System.Text.Json;
using WorkoutDataBase.Entities;

namespace WorkoutDataBase.Repositories
{
    public class JsonRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private static readonly string JsonFilePath = "workouts.json";
        private static readonly string LogFilePath = "workoutsLog.log";
        private static readonly string WorkoutCategoryFilePath = "workoutsCategory.json"; //Will implement soon (probably)

        protected readonly List<T> _items = new();

        public event EventHandler<T>? WorkoutAdded;
        public event EventHandler<T>? WorkoutRemoved;
        public event EventHandler<T>? WorkoutLastUsed;

        public JsonRepository()
        {
            LoadDataCategory();
            LoadData();
        }

        public void AddWorkout(T item)
        {
            int maxId = _items.Count > 0 ? _items.Max(i => i.Id) : 1;
            item.Id = maxId + 1;

            _items.Add(item);
            SaveWorkout();
            WorkoutAdded?.Invoke(this, item);
            LogAudit($"WorkoutAdded {typeof(T).Name} => {item}");
        }

        public void RemoveWorkout(T item)
        {
            _items.Remove(item);
            SaveWorkout();
            WorkoutRemoved?.Invoke(this, item);
            LogAudit($"WorkoutRemoved {typeof(T).Name} => {item}");
        }

        public void LastUsedWorkout(T item, DateTime lastUsedDate)
        {
            if (item is Workout workout)
            {
                workout.LastUsed = lastUsedDate;
            }

            SaveWorkout();
            WorkoutLastUsed?.Invoke(this, item);
            LogAudit($"WorkoutLastUsed updated {typeof(T).Name} => {item}");
        }

        public IEnumerable<T> GetAll()
        {
            return _items.ToList();
        }

        public T GetById(int id)
        {
            return _items.Find(item => item.Id == id);
        }

        public virtual void SaveWorkout()
        {
            try
            {
                var json = JsonSerializer.Serialize<IEnumerable<T>>(_items);
                File.WriteAllText(JsonFilePath, json);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error occoured while saving {exception.Message}");
            }
        }
        public IEnumerable<T> LoadDataCategory()
        {
            if (File.Exists(WorkoutCategoryFilePath))
            {
                var json = File.ReadAllText(WorkoutCategoryFilePath);
                var deserializedJson = JsonSerializer.Deserialize<IEnumerable<T>>(json);
                foreach (var item in deserializedJson)
                {
                    _items.Add(item);
                }
            }
            return _items.ToList();
        }
        public IEnumerable<T> LoadData()
        {
            if (File.Exists(JsonFilePath))
            {
                var json = File.ReadAllText(JsonFilePath);
                var deserializedJson = JsonSerializer.Deserialize<IEnumerable<T>>(json);
                foreach (var item in deserializedJson)
                {
                    _items.Add(item);
                }
            }
            return _items.ToList();
        }

        public void LogAudit(string logEntry)
        {
            File.AppendAllText(LogFilePath, $"[{DateTime.Now:dd.MM.yyyy - HH:mm:ss}] - {logEntry}" + Environment.NewLine);
        }
    }

}
