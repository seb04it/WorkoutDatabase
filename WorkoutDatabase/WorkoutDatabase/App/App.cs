
using WorkoutDataBase.DataProvider;
using WorkoutDataBase.Entities;
using WorkoutDataBase.Repositories;

namespace WorkoutDataBase.App
{
    public class App : IApp
    {
        private readonly IRepository<Workout> _workoutRepository;
        private readonly IWorkoutsProvider _workoutsProvider;

        public App(IRepository<Workout> workoutRepository, IWorkoutsProvider workoutsProvider)
        {
            _workoutRepository = workoutRepository;
            _workoutsProvider = workoutsProvider;
        }

        public void JsonRepositoryWorkoutAdded(object? sender, Workout entity)
        {
            Console.Clear();
            Console.WriteLine($"Workout added => ID.{entity.Id}, {entity.SongName} from {sender?.GetType().Name}");
        }
        public void JsonRepositoryWorkoutRemoved(object? sender, Workout entity)
        {
            Console.Clear();
            Console.WriteLine($"Workout removed => ID.{entity.Id}, {entity.SongName} from {sender?.GetType().Name}");
        }
        public void JsonRepositoryWorkoutLastUsed(object? sender, Workout entity)
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
                if(input == "q")
                {
                    Console.Clear();
                    exitLoop = true;
                }
            }
        }

        public void AddWorkout(IRepository<Workout> workoutRepository)
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
                    _workoutRepository.AddWorkout(new Workout
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

        public void LastUsedWorkout(IRepository<Workout> workoutRepository)
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

        public void RemoveWorkout(IRepository<Workout> workoutRepository)
        {
            Console.WriteLine("\nList Ćwiczeń: ");
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
            catch(Exception exception)
            {
                Console.WriteLine($"Exception caught: {exception.Message}");
            }
        }
    }
}

