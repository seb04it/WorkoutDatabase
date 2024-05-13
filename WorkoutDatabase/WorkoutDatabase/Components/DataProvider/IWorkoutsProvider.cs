using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDatabase.Entities;

namespace WorkoutDatabase.Components.DataProvider
{
    public interface IWorkoutsProvider
    {
        List<WorkoutEntities> Paging(int itemPerPageTake, int page);
        (TimeSpan minWorkoutLength, string SongName, int Id) GetMinimumSongLenght();
        List<string> GetUniqueSongNames();
        List<WorkoutEntities> TakeWorkoutsWhereSongNameStartsWith(string prefix);
        (DateTime LastUsed, string SongName, int Id) GetMostRecentlyUsedWorkout();
    }
}
