using System;
using System.Reflection;
using WorkoutDatabase.Entities;
using WorkoutDatabase.Repositories;

namespace WorkoutDatabase
{
    class Program
    {
        static void Main()
        {
            var workoutRepository = new JsonRepository<Workout>();
            workoutRepository.WorkoutAdded += JsonRepositoryWorkoutAdded;
            workoutRepository.WorkoutRemoved += JsonRepositoryWorkoutRemoved;

            while (true)
            {
                Console.Write("\nWitaj w programie treningowym!\n" +
                "Czego ci potrzeba?\n" +
                "\n1. Wyświetl listę ćwiczeń\n" +
                "2. Dodaj nowe ruch\n" +
                "3. Usuń ruch\n" +
                "4. Dodaj datę ostatniego użycia ćwiczenia\n" +
                "Q. Zamknij program\n" +
                "\nWybór: ");

                var input = Console.ReadLine().ToLower();
                try
                {
                    switch (input)
                    {
                        case "1":
                            WriteAllToConsole(workoutRepository);
                            break;
                        case "2":
                            AddWorkout(workoutRepository);
                            break;
                        case "3":
                            RemoveWorkout(workoutRepository);
                            break;
                        case "4":
                            LastUsedWorkout(workoutRepository);
                            break;
                        case "q":
                            return;
                        default:
                            throw new Exception("Invalid choice, try again");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Exception cought: {exception.Message}");
                }
            }
        }

        static void LastUsedWorkout(JsonRepository<Workout> workoutRepository)
        {
            var items = workoutRepository.GetAll().ToList();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.Write("\nPodaj ID piosenki którą chcesz zmodyfikować: ");
            if (int.TryParse(Console.ReadLine(), out int inputId))
            {
                var workoutToUpdate = workoutRepository.GetById(inputId);
                if (workoutToUpdate != null)
                {
                    Console.Write("\nWybierz sposób dodania daty ostatniego użycia:\n" +
                        "1. \nData dzisiejsza\n" +
                        "2. Samodzielne wpisanie\n" +
                        "\nWybór: ");
                    var choice = Console.ReadLine().ToLower();
                    switch (choice)
                    {
                        case "1":
                            workoutToUpdate.LastUsed = DateTime.Now.Date;
                            workoutRepository.SaveWorkout();
                            Console.WriteLine("Data ostatniego użycia została zaktualizowana!\n");
                            break;
                        case "2":
                            Console.Write("Podaj datę (dd.MM.yyyy): ");
                            if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out var date))
                            {
                                workoutToUpdate.LastUsed = date;

                                workoutRepository.SaveWorkout();
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

        static void WriteAllToConsole(IReadRepository<IEntity> workoutRepository)
        {
            var items = workoutRepository.GetAll().ToList();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }

        static void AddWorkout(IRepository<Workout> workoutRepository)
        {

            Console.Write("\nPodaj kategorię ćwiczeni: ");
            var workoutCategory = Console.ReadLine();
            Console.Write("\nPodaj piosenkę do układu: ");
            var songName = Console.ReadLine();
            Console.Write("\nPodaj długość danego ćwiczenia (w formacie mm':'ss): ");
            try
            {
                if (TimeSpan.TryParseExact(Console.ReadLine(), "mm':'ss", null, out var workoutLenght))
                {
                    workoutRepository.AddWorkout(new Workout
                    {
                        WorkoutCategory = workoutCategory,
                        SongName = songName,
                        WorkoutLenght = workoutLenght
                    });
                    workoutRepository.SaveWorkout();
                    Console.WriteLine("Ćwiczenie dodano pomyślnie!");
                }
                else
                {
                    throw new Exception("Form of time is invalid. Try again (minutes:seconds)");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception cought: {exception.Message}");
            }
        }

        static void RemoveWorkout(IRepository<Workout> workoutRepository)
        {
            var items = workoutRepository.GetAll().ToList();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.Write("Wybierz Id ćwiczenia które chcesz usunąć");
            try
            {
                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    var workoutToRemoveId = items.First(item => item.Id == input);
                    if (workoutToRemoveId != null)
                    {
                        workoutRepository.RemoveWorkout(workoutToRemoveId);
                        workoutRepository.SaveWorkout();
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
                Console.WriteLine($"Exception cought: {exception.Message}");
            }
        }

        static void JsonRepositoryWorkoutAdded(object? sender, Workout entity)
        {
            Console.WriteLine($"Workout added => {entity.WorkoutCategory}, {entity.SongName} from {sender?.GetType().Name}");
        }

        static void JsonRepositoryWorkoutRemoved(object? sender, Workout entity)
        {
            Console.WriteLine($"Workout removed => {entity.WorkoutCategory}, {entity.SongName} from {sender?.GetType().Name}");
        }
    }
}