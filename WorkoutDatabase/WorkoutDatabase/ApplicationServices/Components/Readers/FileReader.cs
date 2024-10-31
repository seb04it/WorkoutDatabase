
using System.Text.Json;
using WorkoutDatabase.ApplicationServices.Components.Readers.Extensions;
using WorkoutDatabase.ApplicationServices.Components.Readers.Models;

namespace WorkoutDatabase.ApplicationServices.Components.Readers
{
    public class FileReader : IFileReader
    {
        public List<Workout> ProcessWorkoutsCsv(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<Workout>();
            }

            var workouts = File.ReadAllLines(filePath)
                .Skip(1)
                .Where(line => line.Length > 1)
                .ToWorkout();

            return workouts.ToList();
        }

        public List<Workout> ProcessWorkoutsJson(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<Workout>();
            }

            var json = File.ReadAllText(filePath);
            var workouts = JsonSerializer.Deserialize<List<Workout>>(json)
                         ?? new List<Workout>();

            return workouts.ToList();
        }
    }


}
