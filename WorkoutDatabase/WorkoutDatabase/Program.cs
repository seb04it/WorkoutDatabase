using System;
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
                Console.Write("Witaj w programie treningowym!\n" +
                "Czego ci potrzeba?\n" +
                "1. Wyświetl listę ćwiczeń\n" +
                "2. Dodaj nowe ruch\n" +
                "3. Usuń ruch\n" +
                "Q. Zamknij program\n" +
                "Wybór: ");

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
                        case "q":
                            return;
                        default:
                            throw new Exception("Invalid choice, try again");
                    }
                }
                catch(Exception exception)
                {
                    Console.WriteLine($"Exception cought: {exception.Message}");
                }
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

            Console.WriteLine("Podaj kategorię ćwiczenia");
            var workoutCategory = Console.ReadLine();
            Console.WriteLine("Podaj piosenkę do układu");
            var songName = Console.ReadLine();
            Console.WriteLine("Podaj długość danego ćwiczenia (w formacie mm':'ss)");
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
            foreach(var item in items)
            {
                Console.WriteLine(item);
            }
            Console.Write("Wybierz Id ćwiczenia które chcesz usunąć");
            try
            {
                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    var workoutToRemove = items.First(item => item.Id == input);
                    if (workoutToRemove != null)
                    {
                        workoutRepository.RemoveWorkout(workoutToRemove);
                        workoutRepository.SaveWorkout();
                        Console.WriteLine("Ćwiczenie usunięte pomyślnie!");
                    }
                }
                else
                {
                    throw new Exception("ID must be an integer");
                }
            }
            catch(Exception exception)
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