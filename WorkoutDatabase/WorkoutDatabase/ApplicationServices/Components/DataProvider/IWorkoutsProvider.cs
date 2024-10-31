
using WorkoutDatabase.DataAccess.Data.Entities;

namespace WorkoutDatabase.ApplicationServices.Components.DataProvider
{
    public interface IWorkoutsProvider
    {
        List<WorkoutEntity> Paging(int itemPerPageTake, int page);
        (TimeSpan minWorkoutLength, string SongName, int Id) GetMinimumSongLenght();
        List<string> GetUniqueSongNames();
        List<WorkoutEntity> TakeWorkoutsWhereSongNameStartsWith(string prefix);
        (DateTime LastUsed, string SongName, int Id) GetMostRecentlyUsedWorkout();
    }
}
