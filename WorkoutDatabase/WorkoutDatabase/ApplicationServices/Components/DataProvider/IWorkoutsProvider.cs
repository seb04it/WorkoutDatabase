using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
