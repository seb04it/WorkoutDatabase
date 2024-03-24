using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutApp.Entities;

namespace WorkoutApp.DataProvider
{
    public interface IWorkoutsProvider
    {
        List<Workout> Paging(int itemPerPageTake, int page);
        (TimeSpan minWorkoutLength, string SongName, int Id) GetMinimumSongLenght();
        List<string> GetUniqueSongNames();
        List<Workout> TakeWorkoutsWhereSongNameStartsWith(string prefix);
        (DateTime LastUsed, string SongName, int Id) GetMostRecentlyUsedWorkout();
    }
}
