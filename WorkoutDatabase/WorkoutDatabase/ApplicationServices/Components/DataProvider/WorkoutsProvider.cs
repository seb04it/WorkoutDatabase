
using WorkoutDatabase.ApplicationServices.Repositories;
using WorkoutDatabase.DataAccess.Data.Entities;

namespace WorkoutDatabase.ApplicationServices.Components.DataProvider
{
    public class WorkoutsProvider : IWorkoutsProvider
    {
        private readonly IRepository<WorkoutEntity> _workoutsRepository;
        public WorkoutsProvider(IRepository<WorkoutEntity> workoutRepository)
        {
            _workoutsRepository = workoutRepository;
        }

        public List<WorkoutEntity> Paging(int itemsPerPage, int page)
        {
            var workouts = _workoutsRepository.GetAll().ToList();
            return workouts
                .Skip(itemsPerPage * (page - 1))
                .Take(itemsPerPage)
                .ToList();
        }

        public (TimeSpan minWorkoutLength, string SongName, int Id) GetMinimumSongLenght()
        {
            var workouts = _workoutsRepository.GetAll().ToList();
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
            var workouts = _workoutsRepository.GetAll().ToList();

            var mostRecentWorkout = workouts
                .Where(x => x.LastUsed.HasValue)
                .FirstOrDefault();

            if (mostRecentWorkout != null)
            {
                return (mostRecentWorkout.LastUsed.Value, mostRecentWorkout.SongName, mostRecentWorkout.Id);
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

        public List<WorkoutEntity> TakeWorkoutsWhereSongNameStartsWith(string prefix)
        {
            var workouts = _workoutsRepository.GetAll().ToList();
            return workouts
                .Where(x => x.SongName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<WorkoutEntity> WhereCategoryIs(string category)
        {
            var workouts = _workoutsRepository.GetAll().ToList();
            return workouts
                .Where(x => x.WorkoutCategory == category)
                .ToList();
        }
    }
}
