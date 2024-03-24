using Microsoft.Extensions.DependencyInjection;
using WorkoutApp.App;
using WorkoutApp.Entities;
using WorkoutApp.Repositories;
using WorkoutApp.UserCommunication;
using WorkoutApp.DataProvider;

var services = new ServiceCollection();
services.AddSingleton<IApp, App>();
services.AddSingleton<IUserCommunication, UserCommunication>();
services.AddSingleton<IRepository<Workout>, JsonRepository<Workout>>();
services.AddSingleton<IWorkoutsProvider, WorkoutsProvider>();

var serviceProvider = services.BuildServiceProvider();
var app = serviceProvider.GetService<IUserCommunication>()!;

var appInstance = serviceProvider.GetService<IApp>()!;
var workoutRepository = serviceProvider.GetService<IRepository<Workout>>()!;
workoutRepository.WorkoutAdded += appInstance.JsonRepositoryWorkoutAdded;
workoutRepository.WorkoutRemoved += appInstance.JsonRepositoryWorkoutRemoved;
workoutRepository.WorkoutLastUsed += appInstance.JsonRepositoryWorkoutLastUsed;

app.Menu();
