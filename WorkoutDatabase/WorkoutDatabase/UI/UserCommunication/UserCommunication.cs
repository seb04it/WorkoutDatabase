using Microsoft.Extensions.DependencyInjection;
using WorkoutDatabase.ApplicationServices.App;
using WorkoutDatabase.ApplicationServices.Repositories;
using WorkoutDatabase.DataAccess.Data;
using WorkoutDatabase.DataAccess.Data.Entities;

namespace WorkoutDatabase.UI.UserCommunication
{
    public class UserCommunication : IUserCommunication
    {
        private readonly IApp _app;
        private readonly IRepository<WorkoutEntity> _workoutRepository;
        private readonly IRepository<WorkoutCategoryEntity> _workoutCategoryEntityRepository;
        private readonly WorkoutDbContext _workoutDbContext;

        public UserCommunication(IApp app, IRepository<WorkoutEntity> workoutRepository, IRepository<WorkoutCategoryEntity> workoutCategoryRepository, WorkoutDbContext workoutDbContext)
        {
            _app = app ?? throw new ArgumentNullException(nameof(app));
            _workoutRepository = workoutRepository ?? throw new ArgumentNullException(nameof(workoutRepository));
            _workoutCategoryEntityRepository = workoutCategoryRepository ?? throw new ArgumentNullException(nameof(workoutCategoryRepository));
            _workoutDbContext = workoutDbContext ?? throw new ArgumentNullException(nameof(workoutDbContext));
            _workoutDbContext.Database.EnsureCreated();
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
                "\n 1. Wyświetl listę ćwiczeńa\n" +
                " 2. Dodaj nowe ruchy\n" +
                " 3. Usuń ruch\n" +
                " 4. Dodaj datę ostatniego użycia ćwiczenia\n" +
                " 5. Edytuj istniejące ćwiczenie\n" +
                " 6. Wyślij dane z pliku do Servera SQL\n" +
                " 7. WorkoutsDataProvider\n" +
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
                            _app.AddWorkout(_workoutRepository, _workoutCategoryEntityRepository);
                            break;
                        case "3":
                            _app.RemoveWorkout(_workoutRepository);
                            break;
                        case "4":
                            _app.LastUsedWorkout(_workoutRepository);
                            break;
                        case "5":
                            _app.EditWorkout(_workoutRepository);
                            break;
                        case "6":
                            _app.SendFileDataToSql();
                            break;
                        case "7":
                            _app.WorkoutProviders();
                            break;
                        case "q":
                            Console.WriteLine("Dziękuję za skorzystanie z programu :)");
                            return;
                        default:
                            Console.Clear();
                            Console.WriteLine("Nieporawny wybór, spróbuj ponownie");
                            break;
                    }
                }
                catch (Exception exception)
                {
                    Console.Clear();
                    Console.WriteLine($"Exception caught: {exception.Message}");
                }
            }
        }

        public void PreMenu(IServiceCollection services)
        {
            if (!File.Exists("workouts.json"))
            {
                using (File.Create("workouts.json")) { }
            }
            if (!File.Exists("workoutsCategories.json"))
            {
                using (File.Create("workoutsCategories.json")) { }
            }

            string currentDirectory = Directory.GetCurrentDirectory();
            string jsonWorkoutFilePath = Path.Combine(currentDirectory, "workouts.json");
            string jsonCategoryFilePath = Path.Combine(currentDirectory, "workoutsCategories.json");

            bool run = true;
            while (run)
            {
                Console.Write("\n\x1b[94m░▒▓█ ░▒▓█▓▒░▒▓█▓▒░▒▓░▒▓█ ░▒▓█▓▒░▒▓█▓▒░▒▓\n\u001b[0m" +
                              "\n\x1b[33m    Witaj w programie treningowym!\n" +
                              "                    -We go jim\n\u001b[0m" +
                              "\n\x1b[94m░▒▓█ ░▒▓█▓▒░▒▓█▓▒░▒▓░▒▓█ ░▒▓█▓▒░▒▓█▓▒░▒▓\n\u001b[0m" +
                              "\nWybierz w jaki sposób chcesz korzystać z aplikacji: " +
                              "\n\n 1. Zapis na serwerze" +
                              "\n 2. Praca na pliku" +
                              "\n\nWybór: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        run = false;
                        services.AddSingleton<IRepository<WorkoutEntity>, SqlRepository<WorkoutEntity>>();
                        services.AddSingleton<IRepository<WorkoutCategoryEntity>>(provider => new JsonRepository<WorkoutCategoryEntity>(jsonCategoryFilePath));
                        Console.Clear();
                        break;
                    case "2":
                        run = false;
                        services.AddSingleton<IRepository<WorkoutEntity>>(provider => new JsonRepository<WorkoutEntity>(jsonWorkoutFilePath));
                        services.AddSingleton<IRepository<WorkoutCategoryEntity>>(provider => new JsonRepository<WorkoutCategoryEntity>(jsonCategoryFilePath));
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Niepoprawny wybór, spróbuj ponownie");
                        break;
                }
            }
        }
    }
}