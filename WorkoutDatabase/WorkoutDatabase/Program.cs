
using System.Globalization;
using WorkoutDatabase.Data;
using WorkoutDatabase.Entities;
using WorkoutDatabase.Repositories;

var workoutRepository = new SqlRepository<Workout>(new WorkoutDbContext());
AddWorkout(workoutRepository);
AddHardWorkout(workoutRepository);
WriteAllToConsole(workoutRepository);


static void AddWorkout(IRepository<Workout> workoutRepository)
{
    workoutRepository.Add(new Workout { WorkoutCategory = "Rozgrzewka", SongName = "All the things she said", WorkoutLenght = TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(46)) });
    workoutRepository.Add(new Workout { WorkoutCategory = "Pośladki", SongName = "Who's Ready for tomorrow", WorkoutLenght = TimeSpan.FromMinutes(1).Add(TimeSpan.FromSeconds(56)) });
    workoutRepository.Add(new Workout { WorkoutCategory = "Triceps", SongName = "Gallowdance (Hardstyle)", WorkoutLenght = TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(24)), LastUsed = DateTime.Parse("01.01.2023, 18:00:00"), TimesUsed = 401 });

    workoutRepository.Save();
}

static void AddHardWorkout(IWriteRepository<HardWorkout> hardWorkoutRepository)
{
    hardWorkoutRepository.Add(new HardWorkout { WorkoutCategory = "Nogi", SongName = "Order", WorkoutLenght = TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(24)), LastUsed = DateTime.Parse("02.01.2023, 18:05:23"), TimesUsed = 395 });
    hardWorkoutRepository.Add(new HardWorkout { WorkoutCategory = "Biceps", SongName = "Can You Feel My Heart (Hardstyle)", WorkoutLenght = TimeSpan.FromMinutes(2).Add(TimeSpan.FromSeconds(33)), LastUsed = DateTime.Parse("02.01.2023, 18:10:80"), TimesUsed = 696123 });
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
