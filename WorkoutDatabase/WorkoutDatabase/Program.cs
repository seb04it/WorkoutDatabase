using Microsoft.Extensions.DependencyInjection;
using WorkoutApp.App;
using WorkoutApp.Entities;
using WorkoutApp.Repositories;
using WorkoutApp.UserCommunication;
using WorkoutApp.DataProvider;
using WorkoutApp.Data;


var services = new ServiceCollection();
services.AddDbContext<WorkoutDbContext>();
services.AddScoped(typeof(IRepository<Workout>), typeof(SqlRepository<Workout>));
services.AddSingleton<IApp, App>();
services.AddSingleton<IUserCommunication, UserCommunication>();
services.AddSingleton<IRepository<Workout>, SqlRepository<Workout>>();
services.AddSingleton<IWorkoutsProvider, WorkoutsProvider>();

var serviceProvider = services.BuildServiceProvider();
var app = serviceProvider.GetService<IUserCommunication>()!;

//var appInstance = serviceProvider.GetService<IApp>()!;
//var workoutRepository = serviceProvider.GetService<IRepository<Workout>>()!;
//workoutRepository.WorkoutAdded += appInstance.JsonRepositoryWorkoutAdded;
//workoutRepository.WorkoutRemoved += appInstance.JsonRepositoryWorkoutRemoved;
//workoutRepository.WorkoutLastUsed += appInstance.JsonRepositoryWorkoutLastUsed;

app.Menu();
