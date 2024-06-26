﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDatabase.Entities;
using WorkoutDatabase.Data.Repositories;

namespace WorkoutDatabase.Components.DataProvider
{
    public class WorkoutsProvider : IWorkoutsProvider
    {
        private readonly IRepository<WorkoutEntities> _workoutsRepository;
        public WorkoutsProvider(IRepository<WorkoutEntities> workoutRepository)
        {
            _workoutsRepository = workoutRepository;
        }

        public List<WorkoutEntities> Paging(int itemsPerPage, int page)
        {
            var workouts = _workoutsRepository.GetAll();
            return workouts
                //.OrderBy(x => x.Id)
                .Skip(itemsPerPage * (page - 1))
                .Take(itemsPerPage)
                .ToList();
        }

        public (TimeSpan minWorkoutLength, string SongName, int Id) GetMinimumSongLenght()
        {
            var workouts = _workoutsRepository.GetAll();
            var minWorkoutLength = workouts
                .Select(x => x.WorkoutLength).Min();
            var minWorkout = workouts
                .First(x => x.WorkoutLength == minWorkoutLength.Value);

            if (minWorkout != null)
            {
                return (minWorkoutLength.Value, minWorkout.SongName, minWorkout.Id);
            }
            else
            {
                return (TimeSpan.Zero, string.Empty, 0);
            }
        }

        public (DateTime LastUsed, string SongName, int Id) GetMostRecentlyUsedWorkout()
        {
            var workouts = _workoutsRepository.GetAll();
            var mostRecentWorkout = workouts
                .Select(x => x.LastUsed).Max();
            var recentWorkout = workouts
                .First(x => x.LastUsed == mostRecentWorkout.Value);
            if (recentWorkout != null)
            {
                return (mostRecentWorkout.Value, recentWorkout.SongName, recentWorkout.Id);
            }
            else
            {
                return (DateTime.MinValue, string.Empty, 0);
            }

        }

        public List<string> GetUniqueSongNames()
        {
            var workouts = _workoutsRepository.GetAll().ToList();
            var songs = workouts
                .Select(x => x.SongName)
                .Distinct()
                .ToList();

            return songs;
        }

        public List<WorkoutEntities> TakeWorkoutsWhereSongNameStartsWith(string prefix)
        {
            var workouts = _workoutsRepository.GetAll();
            return workouts
                .Where(x => x.SongName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<WorkoutEntities> WhereCategoryIs(string category)
        {
            var workouts = _workoutsRepository.GetAll();
            return workouts
                .Where(x => x.WorkoutCategory == category)
                .ToList();
        }



    }
}
