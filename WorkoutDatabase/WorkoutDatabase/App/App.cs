
using WorkoutDatabase.Components.CsvReader;
using WorkoutDatabase.Components.DataProvider;
using WorkoutDatabase.Data;
using WorkoutDatabase.Data.Repositories;
using WorkoutDatabase.Entities;

namespace WorkoutDatabase.App
{
    public class App : IApp
    {
        private readonly IRepository<WorkoutEntities> _workoutRepository;
        private readonly IWorkoutsProvider _workoutsProvider;
        private readonly WorkoutDbContext _workoutDbContext;
        private readonly ICsvReader _csvReader;


        public App(IRepository<WorkoutEntities> workoutRepository, IWorkoutsProvider workoutsProvider, WorkoutDbContext workoutDbContext, ICsvReader csvReader)
        {
            _workoutRepository = workoutRepository;
            _workoutsProvider = workoutsProvider;
            _workoutDbContext = workoutDbContext;
            _csvReader = csvReader;
        }

        public void RepositoryWorkoutAdded(object? sender, WorkoutEntities entity)
        {
            Console.Clear();
            Console.WriteLine($"Workout added => ID.{entity.Id}, {entity.SongName} from {sender?.GetType().Name}");
        }
        public void RepositoryWorkoutRemoved(object? sender, WorkoutEntities entity)
        {
            Console.Clear();
            Console.WriteLine($"Workout removed => ID.{entity.Id}, {entity.SongName} from {sender?.GetType().Name}");
        }
        public void RepositoryWorkoutLastUsed(object? sender, WorkoutEntities entity)
        {
            Console.Clear();
            Console.WriteLine($"WorkoutLastUsed update => ID.{entity.Id}, {entity.SongName} from {sender?.GetType().Name}");
        }

        public void WorkoutProviders()
        {
            bool exitLoop = false;
            while (!exitLoop)
            {
                Console.Clear();

                Console.WriteLine("\n---------------------------------------" +
                "\nTakeWorkoutsWhileSongNameStartsWith()" +
                "\n");

                foreach (var workout in _workoutsProvider.TakeWorkoutsWhereSongNameStartsWith("S"))
                {
                    Console.WriteLine(workout);
                }

                Console.WriteLine("\n---------------------------------------" +
                    "\nGetUniqueSongNames()" +
                    "\n");

                foreach (var workout in _workoutsProvider.GetUniqueSongNames())
                {
                    Console.WriteLine(workout);
                }

                Console.WriteLine("\n---------------------------------------" +
                    "\nGetMinimumSongLenght()" +
                    "\n");

                (TimeSpan minWorkoutLength, string songName, int id) = _workoutsProvider.GetMinimumSongLenght();
                Console.WriteLine($"Najkrótsza piosenka to: " +
                    $"\nId: {id}, Song: {songName}, Time: {minWorkoutLength}");

                Console.WriteLine("\n---------------------------------------" +
                    "\nGetMostRecentlyUsedWorkout()" +
                    "\n");

                (DateTime lastUsed, songName, id) = _workoutsProvider.GetMostRecentlyUsedWorkout();
                Console.WriteLine($"Ostatnia użyta piosenka to: " +
                    $"\nId: {id}, Song: {songName}, Date: {lastUsed}");

                Console.Write("\n\nAby wyjść naciśnij 'Q'\n" +
                                "\nWybór: ");
                var input = Console.ReadLine().ToLower();
                if (input == "q")
                {
                    Console.Clear();
                    exitLoop = true;
                }
            }
        }

        public void AddWorkout(IRepository<WorkoutEntities> workoutRepository)
        {
            Console.Write("\nPodaj kategorię ćwiczenia: ");
            var workoutCategory = Console.ReadLine();
            Console.Write("\nPodaj piosenkę do układu: ");
            var songName = Console.ReadLine();
            Console.Write("\nPodaj artystę utworu: ");
            var artistName = Console.ReadLine();
            Console.Write("\nPodaj długość danego ćwiczenia (w formacie mm':'ss): ");
            try
            {
                if (TimeSpan.TryParseExact(Console.ReadLine(), "mm':'ss", null, out var workoutLenght))
                {
                    _workoutRepository.AddWorkout(new WorkoutEntities
                    {
                        WorkoutCategory = workoutCategory,
                        SongName = songName,
                        ArtistName = artistName,
                        WorkoutLength = workoutLenght
                    });
                    Console.WriteLine($"Ćwiczenie {songName} dodano pomyślnie!");
                }
                else
                {
                    throw new Exception("Form of time is invalid. Try (minutes:seconds)");
                }
            }
            catch (Exception exception)
            {
                Console.Clear();
                Console.WriteLine($"Exception caught: {exception.Message}");
            }
        }

        public void LastUsedWorkout(IRepository<WorkoutEntities> workoutRepository)
        {
            var items = _workoutRepository.GetAll().ToList();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.Write("\nPodaj ID piosenki którą chcesz zmodyfikować: ");
            try
            {
                if (int.TryParse(Console.ReadLine(), out int inputId))
                {
                    var workoutToUpdate = _workoutRepository.GetById(inputId);
                    if (workoutToUpdate != null)
                    {
                        Console.Write("\nWybierz sposób dodania daty ostatniego użycia:\n" +
                            "\n1. Data dzisiejsza\n" +
                            "2. Samodzielne wpisanie\n" +
                            "\nWybór: ");
                        var choice = Console.ReadLine().ToLower();
                        switch (choice)
                        {
                            case "1":
                                _workoutRepository.LastUsedWorkout(workoutToUpdate, DateTime.Now.Date);
                                Console.WriteLine("Data ostatniego użycia została zaktualizowana!\n");
                                break;
                            case "2":
                                Console.Write("Podaj datę (dd.MM.yyyy lub dd:MM:yyyy): ");
                                string inputDate = Console.ReadLine().Replace(':', '.');
                                if (DateTime.TryParseExact(inputDate, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out var date))
                                {
                                    _workoutRepository.LastUsedWorkout(workoutToUpdate, date);
                                    Console.WriteLine("Date ostatniego użycia dodano pomyślnie!");
                                    break;
                                }
                                else
                                {
                                    throw new Exception("Form of date is invalid. Try (day.month.year)");
                                }
                            default:
                                throw new Exception("Invalid choice (Choose 1 or 2)");
                        }
                    }
                }
                else
                {
                    throw new Exception("ID must be an integer");
                }
            }
            catch (Exception exception)
            {
                Console.Clear();
                Console.WriteLine($"Exception caught: {exception.Message}");
            }
        }

        public void RemoveWorkout(IRepository<WorkoutEntities> workoutRepository)
        {
            Console.WriteLine("\nLista Ćwiczeń: ");
            var items = _workoutRepository.GetAll().ToList();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.Write("\nWybierz Id ćwiczenia które chcesz usunąć: ");
            try
            {
                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    var workoutToRemoveId = items.First(item => item.Id == input);
                    if (workoutToRemoveId != null)
                    {
                        _workoutRepository.RemoveWorkout(workoutToRemoveId);
                        Console.WriteLine("Ćwiczenie usunięte pomyślnie!");
                    }
                }
                else
                {
                    throw new Exception("ID must be an integer");
                }
            }
            catch (Exception exception)
            {
                Console.Clear();
                Console.WriteLine($"Exception caught: {exception.Message}");
            }
        }

        public void WriteAllToConsole(IReadRepository<IEntity> workoutRepository)
        {
            Console.Write("\nWybierz sposób wyświetlenia listy:\n" +
                "1. Podział na strony.\n" +
                "2. Wszystko.\n" +
                "\nWybór: ");
            var input = Console.ReadLine().ToLower();
            int page = 1;

            bool exitLoop = false;
            try
            {
                while (!exitLoop)
                {
                    Console.Clear();
                    switch (input)
                    {

                        case "1":
                            DisplayPage(page);
                            Console.Write("\n Naciśnij '<' albo '>' żeby przemieszczać się między stronami (albo 'Q' żeby wyjść): ");
                            string inputPage = Console.ReadLine().ToLower();
                            if (inputPage == "<" && page > 1)
                            {
                                page--;
                            }
                            else if (inputPage == ">")
                            {
                                page++;

                            }
                            else if (inputPage == "q")
                            {
                                Console.Clear();
                                return;
                            }
                            break;

                        case "2":
                            var items = _workoutRepository.GetAll().ToList();
                            Console.WriteLine("\nList wszytkich ćwiczeń: ");
                            foreach (var item in items)
                            {
                                Console.WriteLine(item);
                            }
                            Console.Write("\nAby wyjść naciśnij 'Q'\n" +
                                "\nWybór: ");

                            inputPage = Console.ReadLine().ToLower();
                            if (inputPage == "q")
                            {
                                Console.Clear();
                                exitLoop = true;
                            }
                            break;
                        default:
                            throw new Exception("Invalid choice (Choose 1 or 2)");
                    }


                }
                void DisplayPage(int page)
                {
                    Console.Clear();
                    Console.WriteLine($"\n Stona numer {page}: ");
                    foreach (var workout in _workoutsProvider.Paging(3, page))
                    {
                        Console.WriteLine(workout);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception caught: {exception.Message}");
            }
        }

        public void SendFileDataToSql()
        {
            string filePath = "Resources\\Files\\Workouts.csv";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("The file does not exist at the specified path.");
                return;
            }

            var fileContent = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(fileContent))
            {
                Console.WriteLine("The file is empty.");
                return;
            }
            var workouts = _csvReader.ProcessWorkouts(filePath);

            if (workouts == null || !workouts.Any())
            {
                Console.WriteLine("No workouts found in the CSV file.");
                return;
            }

            foreach (var workout in workouts)
            {
                _workoutDbContext.Workouts.Add(new WorkoutEntities()
                {
                    WorkoutCategory = workout.WorkoutCategory,
                    SongName = workout.SongName,
                    ArtistName = workout.ArtistName,
                    WorkoutLength = workout.WorkoutLength,
                    LastUsed = workout.LastUsed
                });
            }
            _workoutDbContext.SaveChanges();

        }

        public void EditWorkout(IRepository<WorkoutEntities> workoutRepository)
        {
            Console.WriteLine("\nLista Ćwiczeń: ");
            var items = _workoutRepository.GetAll().ToList();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.Write("\nWybierz Id ćwiczenia które chcesz edytować: ");
            try
            {
                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    var workoutToEditId = items.FirstOrDefault(item => item.Id == input);
                    if (workoutToEditId != null)
                    {
                        Console.WriteLine("Którą daną chcesz edytować: " +
                            $"\n1. WorkoutCategory ({workoutToEditId.WorkoutCategory})" +
                            $"\n2. Song ({workoutToEditId.SongName})" +
                            $"\n3. Song Artist ({workoutToEditId.ArtistName})" +
                            $"\n4. Workout Length ({workoutToEditId.WorkoutLength})" +
                            $"\n5. Workout Last Used ({workoutToEditId.LastUsed})");
                        var choice = Console.ReadLine().ToUpper();
                        switch (choice)
                        {
                            case "1":
                                Console.Write($"Podaj zamiennik dla WorkoutCategory ({workoutToEditId.WorkoutCategory}): ");
                                workoutToEditId.WorkoutCategory = Console.ReadLine();
                                break;
                            case "2":
                                Console.Write($"Podaj zamiennik dla Song ({workoutToEditId.SongName}): ");
                                workoutToEditId.SongName = Console.ReadLine();
                                break;
                            case "3":
                                Console.Write($"Podaj zamiennik dla Artist ({workoutToEditId.ArtistName}): ");
                                workoutToEditId.ArtistName = Console.ReadLine();
                                break;
                            case "4":
                                Console.Write($"Podaj zamiennik dla Workout Length ({workoutToEditId.WorkoutLength}): ");
                                var workoutLenght = Console.ReadLine();
                                if (TimeSpan.TryParseExact(Console.ReadLine(), "mm':'ss", null, out var workoutLenghtParsed))
                                {
                                    workoutToEditId.WorkoutLength = workoutLenghtParsed;
                                    break;
                                }
                                else
                                {
                                    throw new Exception("Form of time is invalid. Try (minutes:seconds)");
                                }
                            case "5":
                                Console.WriteLine($"Podaj zamiennik dla Last Used ({workoutToEditId.LastUsed}): ");
                                if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out var date))
                                {
                                    workoutToEditId.LastUsed = date;
                                    break;
                                }
                                else
                                {
                                    throw new Exception("Form of date is invalid. Try (day.month.year)");
                                }        
                        }
                        _workoutRepository.SaveWorkout();
                    }
                }
                else
                {
                    throw new Exception("ID must be an integer");
                }
            }
            catch (Exception exception)
            {
                Console.Clear();
                Console.WriteLine($"Exception caught: {exception.Message}");
            }
        }
    }
}

