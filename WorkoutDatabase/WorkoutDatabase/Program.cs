
using Microsoft.Extensions.DependencyInjection;
using WorkoutDatabase.App;
using WorkoutDatabase.UserCommunication;
using WorkoutDatabase.Data;
using WorkoutDatabase.Entities;
using WorkoutDatabase.Components.DataProvider;
using WorkoutDatabase.Data.Repositories;
using WorkoutDatabase.Components.CsvReader;
using Microsoft.EntityFrameworkCore;

var services = new ServiceCollection();

services.AddSingleton<IUserCommunication, UserCommunication>();
services.AddSingleton<IApp, App>();

//services.AddSingleton<IRepository<WorkoutEntities>, JsonRepository<WorkoutEntities>>();
services.AddSingleton<IRepository<WorkoutEntities>, SqlRepository<WorkoutEntities>>();
services.AddSingleton<IWorkoutsProvider, WorkoutsProvider>();
services.AddSingleton<ICsvReader, CsvReader>();
services.AddDbContext<WorkoutDbContext>(options => options.UseSqlServer("Data Source=DESKTOP-TFF998T\\SQLEXPRESS;Initial Catalog=WorkoutAppStrorage;Integrated Security=True;Encrypt=False"));

var serviceProvider = services.BuildServiceProvider();
var app = serviceProvider.GetService<IUserCommunication>()!;


var appInstance = serviceProvider.GetService<IApp>()!;
var workoutRepository = serviceProvider.GetService<IRepository<WorkoutEntities>>()!;
workoutRepository.WorkoutAdded += appInstance.RepositoryWorkoutAdded;
workoutRepository.WorkoutRemoved += appInstance.RepositoryWorkoutRemoved;
workoutRepository.WorkoutLastUsed += appInstance.RepositoryWorkoutLastUsed;

app.Menu();

//Data Source=DESKTOP-TFF998T\SQLEXPRESS;Initial Catalog=WorkoutAppStrorage;Integrated Security=True;Encrypt=False
