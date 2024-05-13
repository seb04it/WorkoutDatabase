using System.Globalization;
using WorkoutDatabase.Components.CsvReader.Models;

namespace WorkoutDatabase.Components.CsvReader.Extensions
{
    public static class WorkoutExtensions
    {
        public static IEnumerable<Workout> ToWorkout(this IEnumerable<string> source)
        {
            foreach(var line in source)
            {
                var columns = line.Split(',');

                yield return new Workout
                {
                    WorkoutCategory = columns[0],
                    SongName = columns[1],
                    ArtistName = columns[2],
                    WorkoutLength = TimeSpan.ParseExact(columns[3], "h\\:mm\\:ss", CultureInfo.InvariantCulture),
                    LastUsed = DateTime.ParseExact(columns[4], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                };
            }
        }
    }
}
