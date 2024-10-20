using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDatabase.DataAccess.Data.Entities;

namespace WorkoutDatabase.ApplicationServices.Repositories
{
    public interface IReadRepository<out T> where T : class, IEntity
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        void EditItem(int id, Action<T> editAction);
    }
}
