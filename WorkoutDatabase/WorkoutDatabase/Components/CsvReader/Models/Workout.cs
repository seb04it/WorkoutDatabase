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
    }
}
