using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDatabase.ApplicationServices.Repositories;
using WorkoutDatabase.DataAccess.Data.Entities;

namespace WorkoutDatabase.ApplicationServices.App
{
    public interface IApp
    {
        void RepositoryWorkoutAdded(object? sender, WorkoutEntity entity);
        void RepositoryWorkoutRemoved(object? sender, WorkoutEntity entity);
        void RepositoryWorkoutLastUsed(object? sender, WorkoutEntity entity);
        void WorkoutProviders();
        void WriteAllToConsole(IReadRepository<IEntity> workoutRepository);
        void AddWorkout(IRepository<WorkoutEntity> workoutRepository, IRepository<WorkoutCategoryEntity> workoutCategoryRepository);
        void RemoveWorkout(IRepository<WorkoutEntity> workoutRepository);
        void LastUsedWorkout(IRepository<WorkoutEntity> workoutRepository);
        void SendFileDataToSql();
        void EditWorkout(IRepository<WorkoutEntity> workoutRepository);
    }
}
