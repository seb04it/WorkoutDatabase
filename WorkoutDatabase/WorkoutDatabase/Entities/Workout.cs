using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDatabase.Entities
{
    public class Workout : EntityBase
    {
        public string? WorkoutCategory { get; set; }
        public string? SongName { get; set; }
        public TimeSpan? WorkoutLenght { get; set; }
        public DateTime? LastUsed { get; set; }

        public override string ToString() => $"Id {Id}, " +
            $"WorkoutCategory: {WorkoutCategory}, " +
            $"Song: {SongName}, " +
            $"Workout Lenght: {WorkoutLenght?.ToString("mm\\:ss")}, " +
            $"Last Used: {LastUsed?.ToString("dd\\.MM\\.yyyy HH\\:mm\\:ss")}";
    }
}
