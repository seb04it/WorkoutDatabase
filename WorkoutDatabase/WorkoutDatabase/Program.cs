using Microsoft.Extensions.DependencyInjection;
using WorkoutDataBase.App;
using WorkoutDataBase.Entities;
using WorkoutDataBase.Repositories;
using WorkoutDataBase.UserCommunication;
using WorkoutDataBase.DataProvider;
using WorkoutDataBase.Data;


var services = new ServiceCollection();
services.AddDbContext<WorkoutDbContext>();
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
