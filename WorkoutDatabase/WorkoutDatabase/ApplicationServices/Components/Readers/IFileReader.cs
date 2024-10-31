
using WorkoutDatabase.ApplicationServices.Components.Readers.Models;

namespace WorkoutDatabase.ApplicationServices.Components.Readers
{
    public interface IFileReader
    {
        List<Workout> ProcessWorkoutsCsv(string filePath);
        List<Workout> ProcessWorkoutsJson(string filePath);
    }
}
