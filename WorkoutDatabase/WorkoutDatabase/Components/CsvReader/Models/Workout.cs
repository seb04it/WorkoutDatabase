using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDatabase.Components.CsvReader.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public string WorkoutCategory { get; set; }
        public string SongName { get; set; }
        public string ArtistName { get; set; }
        public TimeSpan WorkoutLength { get; set; }
        public DateTime LastUsed { get; set; }
        public override string ToString() => $"Id {Id}, " +
            $"WorkoutCategory: {WorkoutCategory}, " +
            $"Song: {SongName}, " +
            $"Artist: {ArtistName}, " +
            $"Workout Lenght: {WorkoutLength.ToString("mm\\:ss")}, " +
            $"Last Used: {LastUsed.ToString("dd\\.MM\\.yyyy")}";
    }
}
