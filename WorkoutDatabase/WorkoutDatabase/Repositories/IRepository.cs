using WorkoutDatabase.Entities;

namespace WorkoutDatabase.Repositories
{
    public interface IRepository<T> : IWriteRepository<T>, IReadRepository<T>
        where T : class, IEntity
    {
    }
}
