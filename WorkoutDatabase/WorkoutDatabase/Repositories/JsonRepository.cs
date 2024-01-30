
using System.Data;
using System.Text.Json;
using WorkoutDatabase.Entities;

namespace WorkoutDatabase.Repositories
{

    public class JsonRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private static readonly string JsonFilePath = "workouts.json";
        private static readonly string LogFilePath = "workoutsLog.log";

        protected readonly List<T> _items = new();

        public event EventHandler<T>? WorkoutAdded;
        public event EventHandler<T>? WorkoutRemoved;

        public JsonRepository()
        {
            LoadData();
        }

        public void AddWorkout(T item)
        {
            item.Id = _items.Count + 1;
            _items.Add(item);
            WorkoutAdded?.Invoke(this, item);
            LogAudit($"Added {typeof(T).Name} => {item}");
        }

        public void RemoveWorkout(T item)
        {
            _items.Remove(item);
            WorkoutRemoved?.Invoke(this, item);
            LogAudit($"Removed {typeof(T).Name} => {item}");
        }

        public IEnumerable<T> GetAll()
        {
            return _items.ToList();
        }

        public T GetById(int id)
        {
            return _items.Find(item => item.Id == id);
        }

        public void SaveWorkout()
        {
            var json = JsonSerializer.Serialize<IEnumerable<T>>(_items);
            File.WriteAllText(JsonFilePath, json);
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
