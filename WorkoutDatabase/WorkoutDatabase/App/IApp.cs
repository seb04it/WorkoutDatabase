using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDatabase.Entities;
using WorkoutDatabase.Data.Repositories;

namespace WorkoutDatabase.App
{
    public interface IApp
    {
        void RepositoryWorkoutAdded(object? sender, WorkoutEntities entity);
        void RepositoryWorkoutRemoved(object? sender, WorkoutEntities entity);
        void RepositoryWorkoutLastUsed(object? sender, WorkoutEntities entity);
        void WorkoutProviders();
        void WriteAllToConsole(IReadRepository<IEntity> workoutRepository);
        void AddWorkout(IRepository<WorkoutEntities> workoutRepository);
        void RemoveWorkout(IRepository<WorkoutEntities> workoutRepository);
        void LastUsedWorkout(IRepository<WorkoutEntities> workoutRepository);
    }
}
