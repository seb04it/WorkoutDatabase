using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WorkoutDatabase.DataAccess.Data;
using WorkoutDatabase.DataAccess.Data.Entities;
using WorkoutDatabase.ApplicationServices.Components.DataProvider;
using WorkoutDatabase.ApplicationServices.Components.Readers;
using WorkoutDatabase.UI.UserCommunication;
using WorkoutDatabase.ApplicationServices.App;
using WorkoutDatabase.ApplicationServices.Repositories;

var services = new ServiceCollection();

services.AddSingleton<IUserCommunication, UserCommunication>();
services.AddSingleton<IWorkoutsProvider, WorkoutsProvider>();
services.AddSingleton<IFileReader, FileReader>();
services.AddSingleton<IApp, App>();

var connectToSqlString = "Data Source=DESKTOP-1GK64BC\\SQLEXPRESS;Initial Catalog=WorkoutDatabase;Integrated Security=True;Trust Server Certificate=True";
services.AddDbContext<WorkoutDbContext>(options => options.UseSqlServer(connectToSqlString));

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
                                      "\n\n 1. Zapis w chmurze (SQL Server)" +
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

var serviceProvider = services.BuildServiceProvider();
var userCommunication = serviceProvider.GetService<IUserCommunication>()!;
var workoutRepository = serviceProvider.GetService<IRepository<WorkoutEntity>>()!;

var appInstance = serviceProvider.GetService<IApp>()!;
workoutRepository.ItemAdded += appInstance.RepositoryWorkoutAdded;
workoutRepository.ItemRemoved += appInstance.RepositoryWorkoutRemoved;
workoutRepository.ItemLastUsed += appInstance.RepositoryWorkoutLastUsed;

userCommunication.Menu();