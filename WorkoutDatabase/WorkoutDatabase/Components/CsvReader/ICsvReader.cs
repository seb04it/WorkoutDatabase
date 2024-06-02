using WorkoutDatabase.Components.CsvReader.Models;

namespace WorkoutDatabase.Components.CsvReader
{
    public interface ICsvReader
    {
        List<Workout> ProcessWorkouts(string filePath);
        List<Song> ProcessSongs(string filePath);
        List<Car> ProcessCars(string filePath);
        List<Manufacturer> ProcessManufacturers(string filePath);
        List<Workout> ProcessWorkoutsJson(string filePath);
    }
}
