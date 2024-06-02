using WorkoutDatabase.Entities;
using WorkoutDatabase.Data.Repositories;
using WorkoutDatabase.App;
using WorkoutDatabase.Components.CsvReader;
using WorkoutDatabase.Data;

namespace WorkoutDatabase.UserCommunication
{
    public class UserCommunication : IUserCommunication
    {
        private readonly IApp _app;
        private readonly IRepository<WorkoutEntities> _workoutRepository;
        private readonly ICsvReader _csvReader;
        private readonly WorkoutDbContext _workoutDbContext;

        public UserCommunication(ICsvReader csvReader,IApp app, IRepository<WorkoutEntities> workoutRepository, WorkoutDbContext workoutDbContext)
        {
            _app = app;
            _csvReader = csvReader;
            _workoutRepository = workoutRepository;
            _workoutDbContext = workoutDbContext;
            _workoutDbContext.Database.EnsureCreated();
        }

        public void Menu()
        {
            //InsertData();
            //Run();
            StartUpMenu();
        }
        
        public void StartUpMenu()
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
                "5. Edytuj istniejące ćwiczenie\n" +
                "6. Wyślij dane z pliku do Servera SQL\n"+
                "7. WorkoutsDataProvider\n" +
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

//Run();
//{
//InsertData();
//ReadFromDb();
//ReadGroupedWorrkoutsFromDb();
//ChangeWorkout();
//RemoveWorkout();


//}

//private void RemoveWorkout()
//{
//    var song1 = this.ReadFirst("Song1");
//    _workoutDbContext.Remove(song1);
//    _workoutDbContext.SaveChanges();
//}

//private void ChangeWorkout()
//{
//    var song1 = this.ReadFirst("Song1");
//    song1.SongName = "Moja Piosenka";
//    _workoutDbContext.SaveChanges();
//}

//private WorkoutEntities? ReadFirst(string name)
//{
//    return _workoutDbContext.Workouts.FirstOrDefault(workoutSong => workoutSong.SongName == name);
//}

//private void ReadGroupedWorrkoutsFromDb()
//{
//    var groups = _workoutDbContext
//        .Workouts
//        .GroupBy(workout => workout.WorkoutCategory)
//        .Select(workout => new 
//        {
//            WorkoutCategory = workout.Key,
//            Workouts = workout.ToList()
//        })
//        .ToList();

//    foreach(var group in groups)
//    {
//        Console.WriteLine(group.WorkoutCategory);
//        Console.WriteLine("======");
//        foreach (var workout in group.Workouts)
//        {
//            Console.WriteLine($"Song: {workout.SongName}, Artist: {workout.ArtistName}");
//        }
//        Console.WriteLine();
//    }
//}


//private void ReadFromDb()
//{
//    var workoutsFromDb = _workoutDbContext.Workouts.ToList();

//    foreach (var workoutFromDb in workoutsFromDb)
//    {
//        Console.WriteLine($"WorkoutCategory {workoutFromDb.WorkoutCategory}: Song {workoutFromDb.SongName}: Artist {workoutFromDb.ArtistName}");
//    }
//}

