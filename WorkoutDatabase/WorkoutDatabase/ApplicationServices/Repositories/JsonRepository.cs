
using System.Text.Json;
using WorkoutDatabase.DataAccess.Data.Entities;

namespace WorkoutDatabase.ApplicationServices.Repositories
{
    public class JsonRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly string _jsonFilePath;
        private static readonly string LogFilePath = "workoutsJsonLog.log";
        protected readonly List<T> _items = new();

        public event EventHandler<T>? ItemAdded;
        public event EventHandler<T>? ItemRemoved;
        public event EventHandler<T>? ItemUpdated;
        public event EventHandler<T>? ItemLastUsed;

        public JsonRepository(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath;
            LoadData();
        }

        public void AddItem(T item)
        {
            item.Id = _items.Count == 0 ? 1 : _items.Max(i => i.Id) + 1;
            _items.Add(item);
            SaveItem();
            ItemAdded?.Invoke(this, item);
            LogAudit($"Item Added: {typeof(T).Name} => {item}");
        }
        public void EditItem(int id, Action<T> editAction)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            editAction(item);
            SaveItem();
            LogAudit($"Item edited: {typeof(T).Name} => {item}");
        }

        public void RemoveItem(T item)
        {
            _items.Remove(item);
            SaveItem();
            ItemRemoved?.Invoke(this, item);
            LogAudit($"Item Removed: {typeof(T).Name} => {item}");
        }

        public IEnumerable<T> GetAll()
        {
            LogAudit("Items ListTaken");
            return _items.ToList();
        }

        public T GetById(int id)
        {
            LogAudit("Item LookedFor");
            return _items.FirstOrDefault(item => item.Id == id);
        }

        public void UpdateItem(T item, DateTime lastUsedDate)
        {
            if (item is WorkoutEntity workout)
            {
                workout.LastUsed = lastUsedDate;
            }
            SaveItem();
            ItemUpdated?.Invoke(this, item);
            LogAudit($"Item updated: {typeof(T).Name} => {item}");
        }

        private void LoadData()
        {
            var fileContent = File.ReadAllText(_jsonFilePath);
            if (!string.IsNullOrWhiteSpace(fileContent))
            {
                var json = File.ReadAllText(_jsonFilePath);
                var deserializedJson = JsonSerializer.Deserialize<IEnumerable<T>>(json);
                _items.AddRange(deserializedJson);
            }
            LogAudit("Items Loaded");
        }

        public virtual void SaveItem()
        {
            var json = JsonSerializer.Serialize(_items);
            File.WriteAllText(_jsonFilePath, json);
            LogAudit("Items Saved");
        }

        public void LogAudit(string logEntry)
        {
            File.AppendAllText(LogFilePath, $"[{DateTime.Now:dd.MM.yyyy - HH:mm:ss}] - {logEntry}" + Environment.NewLine);
        }
    }
}