using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDatabase.DataAccess.Data.Entities;

namespace WorkoutDatabase.ApplicationServices.Repositories
{
    public interface IWriteRepository<in T> where T : class, IEntity
    {
        void AddItem(T item);
        void RemoveItem(T item);
        void UpdateItem(T item, DateTime lastUsedDate);
        void SaveItem();
    }
}
