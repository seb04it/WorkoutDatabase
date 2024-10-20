
using System.Text.Json;
using WorkoutDatabase.ApplicationServices.Components.CategoryHandler;
using WorkoutDatabase.ApplicationServices.Components.DataProvider;
using WorkoutDatabase.ApplicationServices.Components.Readers;
using WorkoutDatabase.ApplicationServices.Repositories;
using WorkoutDatabase.DataAccess.Data;
using WorkoutDatabase.DataAccess.Data.Entities;

namespace WorkoutDatabase.ApplicationServices.App
{
    public class App : IApp
    {
        private readonly IRepository<WorkoutCategoryEntity> _workoutCategoryRepository;
        private readonly IRepository<WorkoutEntity> _workoutRepository;
        private readonly IWorkoutsProvider _workoutsProvider;
        private readonly WorkoutDbContext _workoutDbContext;
        private readonly IFileReader _FileReader;
        private readonly IWorkoutsHandler _WorkoutsHandler;


        public App(IRepository<WorkoutEntity> workoutRepository, IWorkoutsProvider workoutsProvider, WorkoutDbContext workoutDbContext, IFileReader fileReader, IRepository<WorkoutCategoryEntity> workoutCategoryRepository)
        {
            _workoutRepository = workoutRepository;
            _workoutCategoryRepository = workoutCategoryRepository;
            _workoutsProvider = workoutsProvider;
            _workoutDbContext = workoutDbContext;
            _FileReader = fileReader;
            _WorkoutsHandler = new WorkoutsHandler(workoutCategoryRepository);
        }

        public void RepositoryWorkoutAdded(object? sender, WorkoutEntity entity)
        {
            Console.Clear();
            Console.WriteLine($"Workout added => ID.{entity.Id}, CAT.{entity.WorkoutCategory}, NAME.{entity.SongName} from {sender?.GetType().Name}");
        }
        public void RepositoryWorkoutRemoved(object? sender, WorkoutEntity entity)
        {
            Console.Clear();
            Console.WriteLine($"Workout removed => ID.{entity.Id}, CAT.{entity.WorkoutCategory}, NAME.{entity.SongName} from {sender?.GetType().Name}");
        }
        public void RepositoryWorkoutLastUsed(object? sender, WorkoutEntity entity)
        {
            Console.Clear();
            Console.WriteLine($"WorkoutLastUsed update => ID.{entity.Id}, CAT.{entity.WorkoutCategory}, NAME.{entity.SongName} from {sender?.GetType().Name}");
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

        public void AddWorkout(IRepository<WorkoutEntity> workoutRepository, IRepository<WorkoutCategoryEntity> workoutCategoryRepository)
        {
            Console.Write("\nPodaj kategorię ćwiczenia: \n\n");
            string workoutCategory = _WorkoutsHandler.CategoryChoice();
            Console.Write("\nPodaj piosenkę do układu: ");
            var songName = Console.ReadLine();
            Console.Write("\nPodaj artystę utworu: ");
            var artistName = Console.ReadLine();
            Console.Write("\nPodaj długość danego ćwiczenia (w formacie mm':'ss): ");
            if (TimeSpan.TryParseExact(Console.ReadLine(), "mm':'ss", null, out var workoutLength))
            {
                workoutRepository.ItemAdded += RepositoryWorkoutAdded;
                _workoutRepository.AddItem(new WorkoutEntity
                {
                    WorkoutCategory = workoutCategory,
                    SongName = songName,
                    ArtistName = artistName,
                    WorkoutLength = workoutLength
                });
                workoutRepository.ItemAdded -= RepositoryWorkoutAdded;
            }
            else
            {
                throw new Exception("Form of time is invalid. Try (minutes:seconds)");
            }

        }
        public void LastUsedWorkout(IRepository<WorkoutEntity> workoutRepository)
        {

            var items = _workoutRepository.GetAll().ToList();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.Write("\nPodaj ID piosenki którą chcesz zmodyfikować: ");

            if (int.TryParse(Console.ReadLine(), out int inputId))
            {
                var workoutToUpdate = _workoutRepository.GetById(inputId);
                if (workoutToUpdate != null)
                {
                    DateTime workoutLastUsed = _WorkoutsHandler.DateTimeUpdate();
                    _workoutRepository.UpdateItem(workoutToUpdate, workoutLastUsed);
                    Console.Clear();
                    Console.WriteLine("Date ostatniego użycia dodano pomyślnie!");
                }
                else
                {
                    throw new Exception("The ID you chose doesn't exist");
                }
            }
            else
            {
                throw new Exception("ID must be an integer");
            }

        }

        public void RemoveWorkout(IRepository<WorkoutEntity> workoutRepository)
        {
            Console.WriteLine("\nLista Ćwiczeń: ");
            var items = _workoutRepository.GetAll().ToList();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.Write("\nWybierz Id ćwiczenia które chcesz usunąć: ");

            if (int.TryParse(Console.ReadLine(), out int input))
            {
                var workoutToRemoveId = items.First(item => item.Id == input);
                if (workoutToRemoveId != null)
                {
                    workoutRepository.ItemRemoved += RepositoryWorkoutAdded;
                    _workoutRepository.RemoveItem(workoutToRemoveId);
                    Console.WriteLine("Ćwiczenie usunięte pomyślnie!");
                    workoutRepository.ItemRemoved -= RepositoryWorkoutAdded;
                }
            }
            else
            {
                throw new Exception("ID must be an integer");
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

        public void SendFileDataToSql()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "workouts.json");

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
            var workouts = _FileReader.ProcessWorkoutsJson(filePath);

            if (workouts == null || !workouts.Any())
            {
                Console.WriteLine("No workouts found in the file.");
                return;
            }

            foreach (var workout in workouts)
            {
                _workoutDbContext.Workouts.Add(new WorkoutEntity()
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

        public void EditWorkout(IRepository<WorkoutEntity> workoutRepository)
        {
            Console.WriteLine("\nLista Ćwiczeń: \n");
            var items = _workoutRepository.GetAll().ToList();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.Write("\nWybierz Id ćwiczenia które chcesz edytować: ");

            if (int.TryParse(Console.ReadLine(), out int input))
            {
                var workoutToEditId = workoutRepository.GetById(input);
                if (workoutToEditId != null)
                {
                    Console.Write("\nKtórą daną chcesz edytować: " +
                        $"\n\n1. WorkoutCategory ({workoutToEditId.WorkoutCategory})" +
                        $"\n2. Song ({workoutToEditId.SongName})" +
                        $"\n3. Song Artist ({workoutToEditId.ArtistName})" +
                        $"\n4. Workout Length ({workoutToEditId.WorkoutLength})" +
                        $"\n5. Workout Last Used ({workoutToEditId.LastUsed})" +
                        $"\n\nWybór: ");
                    var choice = Console.ReadLine().ToUpper();
                    _workoutRepository.EditItem(input, workout =>
                    {
                        switch (choice)
                        {
                            case "1":
                                Console.Write($"\nPodaj kategorię ćwiczenia zamiast {workout.WorkoutCategory}: \n\n");
                                workout.WorkoutCategory = _WorkoutsHandler.CategoryChoice();
                                break;
                            case "2":
                                Console.Write($"Podaj zamiennik dla Song ({workout.SongName}): ");
                                workout.SongName = Console.ReadLine();
                                break;
                            case "3":
                                Console.Write($"Podaj zamiennik dla Artist ({workout.ArtistName}): ");
                                workout.ArtistName = Console.ReadLine();
                                break;
                            case "4":
                                Console.Write($"Podaj zamiennik dla Workout Length ({workout.WorkoutLength}): ");
                                var workoutLenght = Console.ReadLine();
                                if (TimeSpan.TryParseExact(Console.ReadLine(), "mm':'ss", null, out var workoutLenghtParsed))
                                {
                                    workout.WorkoutLength = workoutLenghtParsed;
                                    break;
                                }
                                else
                                {
                                    throw new Exception("Form of time is invalid. Try (minutes:seconds)");
                                }
                            case "5":
                                DateTime workoutLastUsed = _WorkoutsHandler.DateTimeUpdate();
                                _workoutRepository.UpdateItem(workoutToEditId, workoutLastUsed);
                                Console.Clear();
                                Console.WriteLine("Date ostatniego użycia dodano pomyślnie!");
                                break;
                        }
                    });
                }
                else
                {
                    throw new Exception("ID must be an integer");
                }

            }
        }
    }
}

