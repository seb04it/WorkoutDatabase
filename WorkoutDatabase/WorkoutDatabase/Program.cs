using Microsoft.Extensions.DependencyInjection;
using WorkoutDatabase.App;
using WorkoutDatabase.UserCommunication;
using WorkoutDatabase.Data;
using WorkoutDatabase.Entities;
using WorkoutDatabase.Components.DataProvider;
using WorkoutDatabase.Data.Repositories;
using WorkoutDatabase.Components.CsvReader;

var services = new ServiceCollection();
services.AddDbContext<WorkoutDbContext>();
services.AddSingleton<IApp, App>();
services.AddSingleton<IUserCommunication, UserCommunication>();
services.AddSingleton<IRepository<WorkoutEntities>, JsonRepository<WorkoutEntities>>();
services.AddSingleton<IWorkoutsProvider, WorkoutsProvider>();
services.AddSingleton<ICsvReader, CsvReader>();

var serviceProvider = services.BuildServiceProvider();
var app = serviceProvider.GetService<IUserCommunication>()!;

var appInstance = serviceProvider.GetService<IApp>()!;
var workoutRepository = serviceProvider.GetService<IRepository<WorkoutEntities>>()!;
workoutRepository.WorkoutAdded += appInstance.RepositoryWorkoutAdded;
workoutRepository.WorkoutRemoved += appInstance.RepositoryWorkoutRemoved;
workoutRepository.WorkoutLastUsed += appInstance.RepositoryWorkoutLastUsed;

app.Menu();

//
