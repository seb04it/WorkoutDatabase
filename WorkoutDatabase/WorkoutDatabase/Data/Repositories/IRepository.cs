using WorkoutDatabase.Entities;

namespace WorkoutDatabase.Data.Repositories
{
    public interface IRepository<T> : IWriteRepository<T>, IReadRepository<T>
        where T : class, IEntity
    {
        public event EventHandler<T>? WorkoutAdded;
        public event EventHandler<T>? WorkoutRemoved;
        public event EventHandler<T>? WorkoutLastUsed;
    }
}
