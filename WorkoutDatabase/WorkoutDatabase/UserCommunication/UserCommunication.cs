using WorkoutDatabase.Entities;
using WorkoutDatabase.Data.Repositories;
using WorkoutDatabase.App;
using System.Xml.Linq;
using WorkoutDatabase.Components.CsvReader;
using System.Linq;
using WorkoutDatabase.Components.CsvReader.Models;
using System.Reflection.Metadata;

namespace WorkoutDatabase.UserCommunication
{
    public class UserCommunication : IUserCommunication
    {
        private readonly ICsvReader _csvReader;
        private readonly IApp _app;
        private readonly IRepository<WorkoutEntities> _workoutRepository;

        public UserCommunication(ICsvReader csvReader, IApp app, IRepository<WorkoutEntities> workoutRepository)
        {
            _app = app;
            _csvReader = csvReader;
            _workoutRepository = workoutRepository;
        }

        public void Menu()
        {
            XmlMethods();
            //CreateCarsXml();
            //CreateWorkoutsXml();
            //QueryCarsXml();
            Console.WriteLine();
            StartUpMenu();
        }

        private void XmlMethods()
        {
            static void QueryCarsXml()
            {
                var document = XDocument.Load("Cars.xml");
                var names = document
                    .Descendants("Manufacturer")
                    .Select(manufacturer => manufacturer.Attribute("Name")?.Value);

                foreach (var name in names)
                {
                    Console.WriteLine(name);
                }
            }

            void CreateWorkoutsXml()
            {
                var workouts = _csvReader.ProcessWorkouts("Resources\\Files\\Workouts.csv");
                var songs = _csvReader.ProcessSongs("Resources\\Files\\Songs.csv");


                var groups = workouts.GroupBy(
                w => new { w.WorkoutCategory, w.ArtistName },
               (key, g) => new
               {
                   WorkoutCategory = key.WorkoutCategory,
                   ArtistName = key.ArtistName,
                   Songs = g.Select(x => x.SongName).Distinct().ToList()
               });

                var document = new XDocument();
                var workoutsGroup = new XElement("WorkoutCategories", groups
                    .Select(g =>
                    new XElement("WorkoutCategory",
                        new XAttribute("Name", g.WorkoutCategory),
                        new XAttribute("CombinedSongsCount", g.Songs.Count()),
                        new XAttribute("Artist", g.ArtistName),
                            new XElement("Songs", g.Songs
                            .Select(song =>
                            new XElement("Song",
                                new XAttribute("SongName", song)))))));

                document.Add(workoutsGroup);
                document.Save("Workout.xml");


            }

            void CreateCarsXml()
            {
                var cars = _csvReader.ProcessCars("Resources\\Files\\fuel.csv");
                var manufacturers = _csvReader.ProcessManufacturers("Resources\\Files\\manufacturers.csv");

                var groups = manufacturers.GroupJoin(
                cars,
                manufacturer => manufacturer.Name,
                car => car.Manufacturer,
                (m, g) => new
                {
                    Manufacturer = m,
                    Car = g
                });

                var document = new XDocument();
                var manufacturersGroup = new XElement("Manufacturers", groups
                    .Select(m =>
                        new XElement("Manufacturer",
                            new XAttribute("Name", m.Manufacturer.Name!),
                            new XAttribute("Country", m.Manufacturer.Country!),
                                new XElement("Cars",
                                    new XAttribute("Country", m.Manufacturer.Country!),
                                    new XAttribute("CombinedSum", m.Car.Sum(c => c.Combined)),
                                        new XElement("Car", m.Car
                                            .Select(g =>
                                           new XElement("Car",
                                               new XAttribute("Model", g.Name!),
                                               new XAttribute("Combined", g.Combined))))))));

                document.Add(manufacturersGroup);
                document.Save("Cars.xml");
            }
        }

        public void StartUpMenu()
        {
            while (true)
            {
                Console.Write("\n\x1b[94m░▒▓█ ░▒▓█▓▒░▒▓█▓▒░▒▓░▒▓█ ░▒▓█▓▒░▒▓█▓▒░▒▓\n\u001b[0m" +
                    "\n\x1b[33m    Witaj w programie treningowym!\n" +
                    "                    -We go jim\n\u001b[0m" +
                "\n\x1b[94m░▒▓█ ░▒▓█▓▒░▒▓█▓▒░▒▓░▒▓█ ░▒▓█▓▒░▒▓█▓▒░▒▓\n\u001b[0m" +
                "\nCzego ci potrzeba?\n" +
                "\n1. Wyświetl listę ćwiczeńa\n" +
                "2. Dodaj nowe ruchy\n" +
                "3. Usuń ruch\n" +
                "4. Dodaj datę ostatniego użycia ćwiczenia\n" +
                "5. WorkoutsDataProvider\n" +
                "Q. Zamknij program\n" +
                "\nWybór: ");

                var input = Console.ReadLine().ToLower();
                try
                {
                    switch (input)
                    {
                        case "1":
                            _app.WriteAllToConsole(_workoutRepository);
                            break;
                        case "2":
                            _app.AddWorkout(_workoutRepository);
                            break;
                        case "3":
                            _app.RemoveWorkout(_workoutRepository);
                            break;
                        case "4":
                            _app.LastUsedWorkout(_workoutRepository);
                            break;
                        case "5":
                            _app.WorkoutProviders();
                            break;
                        case "q":
                            return;
                        default:
                            throw new Exception("Invalid choice, try again");
                    }
                }
                catch (Exception exception)
                {
                    Console.Clear();
                    Console.WriteLine($"Exception cought: {exception.Message}");
                }
            }
        }

        private static void QueryCarsXml()
        {
            var document = XDocument.Load("Cars.xml");
            var names = document
                .Descendants("Manufacturer")
                .Select(manufacturer => manufacturer.Attribute("Name")?.Value);

            foreach (var name in names)
            {
                Console.WriteLine(name);
            }
        }

        private void CreateWorkoutsXml()
        {
            var workouts = _csvReader.ProcessWorkouts("Resources\\Files\\Workouts.csv");
            var songs = _csvReader.ProcessSongs("Resources\\Files\\Songs.csv");


            var groups = workouts.GroupBy(
            w => new { w.WorkoutCategory, w.ArtistName },
           (key, g) => new
           {
               WorkoutCategory = key.WorkoutCategory,
               ArtistName = key.ArtistName,
               Songs = g.Select(x => x.SongName).Distinct().ToList()
           });

            var document = new XDocument();
            var workoutsGroup = new XElement("WorkoutCategories", groups
                .Select(g =>
                new XElement("WorkoutCategory",
                    new XAttribute("Name", g.WorkoutCategory),
                    new XAttribute("CombinedSongsCount", g.Songs.Count()),
                    new XAttribute("Artist", g.ArtistName),
                        new XElement("Songs", g.Songs
                        .Select(song =>
                        new XElement("Song",
                            new XAttribute("SongName", song)))))));

            document.Add(workoutsGroup);
            document.Save("Workout.xml");


        }

        private void CreateCarsXml()
        {
            var cars = _csvReader.ProcessCars("Resources\\Files\\fuel.csv");
            var manufacturers = _csvReader.ProcessManufacturers("Resources\\Files\\manufacturers.csv");

            var groups = manufacturers.GroupJoin(
            cars,
            manufacturer => manufacturer.Name,
            car => car.Manufacturer,
            (m, g) => new
            {
                Manufacturer = m,
                Car = g
            });

            var document = new XDocument();
            var manufacturersGroup = new XElement("Manufacturers", groups
                .Select(m =>
                    new XElement("Manufacturer",
                        new XAttribute("Name", m.Manufacturer.Name!),
                        new XAttribute("Country", m.Manufacturer.Country!),
                            new XElement("Cars",
                                new XAttribute("Country", m.Manufacturer.Country!),
                                new XAttribute("CombinedSum", m.Car.Sum(c => c.Combined)),
                                    new XElement("Car", m.Car
                                        .Select(g =>
                                       new XElement("Car",
                                           new XAttribute("Model", g.Name!),
                                           new XAttribute("Combined", g.Combined))))))));

            document.Add(manufacturersGroup);
            document.Save("Cars.xml");
        }
    }
}