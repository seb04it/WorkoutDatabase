﻿
using WorkoutDataBase.Entities;

namespace WorkoutDataBase.Repositories
{
    public interface IWriteRepository<in T> where T : class, IEntity
    {
        void AddWorkout(T item);
        void RemoveWorkout(T item);
        void LastUsedWorkout(T item, DateTime lastUsedDate);
        void SaveWorkout();
    }
}
