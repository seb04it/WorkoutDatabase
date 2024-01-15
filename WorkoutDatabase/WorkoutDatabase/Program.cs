
using WorkoutDatabase.Data;
using WorkoutDatabase.Entities;
using WorkoutDatabase.Repositories;

var workoutRepository = new SqlRepository<Workout>(new WorkoutDbContext());
AddWorkout(workoutRepository);
AddHardWorkout(workoutRepository);
WriteAllToConsole(workoutRepository);


static void AddWorkout(IRepository<Workout> workoutRepository)
{
    workoutRepository.Add(new Workout { WorkoutCategory = "Rozgrzewka", SongName = "All the things she said", WorkoutLenght = "3:46" });
    workoutRepository.Add(new Workout { WorkoutCategory = "Pośladki", SongName = "Who's Ready for tomorrow", WorkoutLenght = "1:56" });
    workoutRepository.Add(new Workout { WorkoutCategory = "Biceps", SongName = "Gallowdance (Hardstyle)", WorkoutLenght = "3:24", LastUsed = "13.01.24", TimesUsed = 401 });

    workoutRepository.Save();
}

static void AddHardWorkout(IWriteRepository<HardWorkout> hardWorkoutRepository)
{
    hardWorkoutRepository.Add(new HardWorkout { WorkoutCategory = "Nogi", SongName = "Order", WorkoutLenght = "7:00", LastUsed = "13.01.24", TimesUsed = 395 });
    hardWorkoutRepository.Save();
}

static void WriteAllToConsole(IReadRepository<IEntity> repository)
{
    var items = repository.GetAll();
    foreach (var item in items)
    {
        Console.WriteLine(item);
    }
}
