using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDatabase.Entities;

namespace WorkoutDatabase.Repositories
{
    public class ListRepository<T>
        where T : class, IEntity, new()
    {
        protected readonly List<T> _items = new();
        
        public IEnumerable<T> GetAll()
        {
            return _items.ToList();
        }

        public T GetById(int id)
        {
            return _items.Single(item => item.Id == id);
        }

        public void AddWorkout(T item)
        {
            item.Id = _items.Count + 1;
            _items.Add(item);
        }

        public void RemoveWorkout(T item)
        {
            _items.Remove(item);
        }
    }
}
