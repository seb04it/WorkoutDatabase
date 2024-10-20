using WorkoutDatabase.DataAccess.Data.Entities;

namespace WorkoutDatabase.ApplicationServices.Repositories
{
    public interface IRepository<T> : IWriteRepository<T>, IReadRepository<T>
        where T : class, IEntity
    {
        public event EventHandler<T>? ItemAdded;
        public event EventHandler<T>? ItemRemoved;
        public event EventHandler<T>? ItemLastUsed;
    }
}
