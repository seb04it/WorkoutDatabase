using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutApp.Entities;
using WorkoutApp.Repositories;

namespace WorkoutApp.App
{
    public interface IApp
    {
        void JsonRepositoryWorkoutAdded(object? sender, Workout entity);
        void JsonRepositoryWorkoutRemoved(object? sender, Workout entity);
        void JsonRepositoryWorkoutLastUsed(object? sender, Workout entity);
        void WorkoutProviders();
        void WriteAllToConsole(IReadRepository<IEntity> workoutRepository);
        void AddWorkout(IRepository<Workout> workoutRepository);
        void RemoveWorkout(IRepository<Workout> workoutRepository);
        void LastUsedWorkout(IRepository<Workout> workoutRepository);
    }
}
