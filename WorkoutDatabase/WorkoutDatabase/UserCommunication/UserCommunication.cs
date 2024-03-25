
using WorkoutApp.App;
using WorkoutApp.Entities;
using WorkoutApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WorkoutApp.UserCommunication
{
    public class UserCommunication : IUserCommunication
    {
        private readonly IApp _app;
        private readonly IRepository<Workout> _workoutRepository;
        private readonly DbContext _dbContext;

        public UserCommunication(IApp app, IRepository<Workout> workoutRepository, DbContext dbContext)
        {
            _app = app;
            _workoutRepository = workoutRepository;
            _dbContext = dbContext;
        }

        public void Menu()
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
    }
}
