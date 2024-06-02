
using WorkoutDatabase.Components.CsvReader.Models;
using WorkoutDatabase.Components.CsvReader.Extensions;
using System.Globalization;
using System.Text.Json;

namespace WorkoutDatabase.Components.CsvReader
{
    public class CsvReader : ICsvReader
    {
        public List<Car> ProcessCars(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<Car>();
            }

            var cars = File.ReadAllLines(filePath)
                .Skip(1)
                .Where(line => line.Length > 1)
                .Select(line =>
                {
                    var columns = line.Split(',');

                    return new Car
                    {
                        Year = int.Parse(columns[0]),
                        Manufacturer = columns[1],
                        Name = columns[2],
                        Displacement = double.Parse(columns[3], CultureInfo.InvariantCulture),
                        Cylinders = int.Parse(columns[4]),
                        City = int.Parse(columns[5]),
                        Highway = int.Parse(columns[6]),
                        Combined = int.Parse(columns[7])
                    };
                });

            return cars.ToList();
            

        }

        public List<Manufacturer> ProcessManufacturers(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<Manufacturer>();
            }

            var manufacturers = File.ReadAllLines(filePath)
                .Where(line => line.Length > 1)
                .Select(line =>
                {
                    var columns = line.Split(',');

                    return new Manufacturer
                    {
                        Name = columns[0],
                        Country = columns[1],
                        Year = int.Parse(columns[2])
                    };
                });

            return manufacturers.ToList();
        }

        public List<Song> ProcessSongs(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<Song>();
            }

            var songs = File.ReadAllLines(filePath)
                .Where(line => line.Length > 1)
                .Select(line =>
                {
                    var columns = line.Split(',');

                    return new Song
                    {
                        Name = columns[0],
                        Artist = columns[1],
                        Style = columns[2]
                    };
                });

            return songs.ToList();
        }

        public List<Workout> ProcessWorkouts(string filePath)
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

            try
            {
                var json = File.ReadAllText(filePath);
                var workouts = JsonSerializer.Deserialize<List<Workout>>(json);
                return workouts ?? new List<Workout>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while reading JSON: {ex.Message}");
                return new List<Workout>();
            }
        }
    }


}
