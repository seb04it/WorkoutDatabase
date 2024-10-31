
namespace WorkoutDatabase.ApplicationServices.Components.Readers.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public string WorkoutCategory { get; set; }
        public string SongName { get; set; }
        public string ArtistName { get; set; }
        public TimeSpan WorkoutLength { get; set; }
        public DateTime? LastUsed { get; set; }
    }
}
