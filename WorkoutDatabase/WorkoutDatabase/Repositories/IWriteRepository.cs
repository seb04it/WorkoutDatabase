using WorkoutDatabase.Entities;

namespace WorkoutDatabase.Repositories
{
    public interface IWriteRepository<in T> where T : class, IEntity
    {
        void AddWorkout(T item);
        void RemoveWorkout(T item);
        void SaveWorkout();
    }
}
